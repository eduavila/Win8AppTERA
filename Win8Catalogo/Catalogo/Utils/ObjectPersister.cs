using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.ApplicationModel;
using Windows.Foundation;

namespace Win8Catalogo.Catalogo.Utils
{
    public enum ObjectPersisterLocation
    { 
        Local,
        Roaming,
        Temp,
        Path,
    }

    public static class ObjectPersister
    {
        public static async Task SaveAsync(string name, ObjectPersisterLocation location, object obj, bool preserveObjectReferences = true, bool useMetadataFile = false)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            var folder = await GetStorageFolder(name, location);
            name = Path.GetFileName(name);

            var typeList = GetKnownTypesByObject(obj);

            if (useMetadataFile)
            {
                var metaFile = await folder.CreateFileAsync(name + ".meta", CreationCollisionOption.ReplaceExisting);
                using (var stream = await metaFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var typeNamesList = typeList.Select((t) => t.FullName).ToList();
                    var serializer = new DataContractSerializer(typeof(List<string>));
                    serializer.WriteObject(stream.AsStreamForWrite(), typeNamesList);
                    await stream.FlushAsync();
                }
            }

            var file = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var settings = new DataContractSerializerSettings();
                settings.PreserveObjectReferences = preserveObjectReferences;
                settings.KnownTypes = typeList;
                var serializer = new DataContractSerializer(obj.GetType(), settings);
                serializer.WriteObject(stream.AsStreamForWrite(), obj);
                await stream.FlushAsync();
            }
        }

        public static async Task<T> LoadAsync<T>(string name, ObjectPersisterLocation location, bool preserveObjectReferences = true, bool useMetadataFile = false)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            var folder = await GetStorageFolder(name, location);
            name = Path.GetFileName(name);

            try
            {
                var file = await folder.GetFileAsync(name);
                using (var stream = await file.OpenReadAsync())
                {
                    var settings = new DataContractSerializerSettings();
                    settings.PreserveObjectReferences = preserveObjectReferences;
                    if (useMetadataFile)
                    {
                        settings.KnownTypes =  await GetTypeListWithMetadataFile(name, folder);                    
                    }
                    if (settings.KnownTypes == null)
                    {
                        settings.KnownTypes = GetKnownTypesByType(typeof(T));
                    }
                    var serializer = new DataContractSerializer(typeof(T), settings);
                    return (T)(serializer.ReadObject(stream.AsStreamForRead()));
                }
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
        }


        private static async Task<StorageFolder> GetStorageFolder(string name, ObjectPersisterLocation location)
        {
            StorageFolder folder = null;
            switch (location)
            {
                case ObjectPersisterLocation.Local:
                    folder = ApplicationData.Current.LocalFolder;
                    break;
                case ObjectPersisterLocation.Roaming:
                    folder = ApplicationData.Current.RoamingFolder;
                    break;
                case ObjectPersisterLocation.Temp:
                    folder = ApplicationData.Current.TemporaryFolder;
                    break;
                case ObjectPersisterLocation.Path:
                    folder = await StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(name));
                    break;
            }
            return folder;
        }

        private static async Task<List<Type>> GetTypeListWithMetadataFile(string fileName, StorageFolder folder)
        {
            List<Type> typeList = null;
            try
            {
                var metaFile = await folder.GetFileAsync(fileName + ".meta");
                using (var stream = await metaFile.OpenReadAsync())
                {
                    var serializer = new DataContractSerializer(typeof(List<String>));
                    var typeNamesList = (List<String>)(serializer.ReadObject(stream.AsStreamForRead()));
                    typeList = typeNamesList.Select((s) => Type.GetType(s)).ToList();
                }
            }
            catch (FileNotFoundException) { }

            if (typeList == null)
            {
                try
                {
                    var metaFile = await folder.GetFileAsync("meta.meta");
                    using (var stream = await metaFile.OpenReadAsync())
                    {
                        var serializer = new DataContractSerializer(typeof(List<String>));
                        var typeNamesList = (List<String>)(serializer.ReadObject(stream.AsStreamForRead()));
                        typeList = typeNamesList.Select((s) => Type.GetType(s)).ToList();
                    }
                }
                catch (FileNotFoundException) { }
            }

            return typeList;
        }

        private static List<Type> GetKnownTypesByObject(object obj)
        {
            var types = new Dictionary<Type, Type>();
            AddKnownTypesByObject(obj, types);
            return types.Keys.ToList();
        }

        private static Dictionary<Type, List<Type>> KnownTypesDictionary = new Dictionary<Type, List<Type>>();

        private static List<Type> GetKnownTypesByType(Type type)
        {
            if (!KnownTypesDictionary.ContainsKey(type))
            {
                var types = new Dictionary<Type, Type>();
                AddKnownTypesByType(type, types);
                KnownTypesDictionary[type] = types.Keys.ToList();
            }
            return KnownTypesDictionary[type];
        }

        private static void AddKnownTypesByObject(object obj, Dictionary<Type, Type> types)
        {
            if (obj == null)
                return;
            var type = obj.GetType();
            var typeInfo = type.GetTypeInfo();
            if (type == typeof(string) || typeInfo.IsPrimitive)
                return;
            types[type] = type;
            foreach (var field in typeInfo.DeclaredFields)
            {
                try
                {
                    if (field.IsPublic && !field.IsStatic)
                    {
                        var value = field.GetValue(obj);
                        if (value != null)
                        {
                            AddKnownTypesByObject(value, types);
                        }
                        else
                        {
                            AddKnownTypesByType(field.FieldType, types);
                        }
                    }
                }
                catch { }
            }
            foreach (var property in typeInfo.DeclaredProperties)
            {
                try
                {
                    if (property.CanRead)
                    {
                        var value = property.GetValue(obj);
                        if (value != null)
                        {
                            AddKnownTypesByObject(value, types);
                        }
                        else
                        {
                            AddKnownTypesByType(property.PropertyType, types);
                        }
                    }
                }
                catch { }
            }
        }

        private static void AddKnownTypesByType(Type type, Dictionary<Type, Type> types)
        {
            var typeInfo = type.GetTypeInfo();
            if (type == typeof(string) || typeInfo.IsPrimitive || types.ContainsKey(type))
                return;
            types[type] = type;
            foreach (var field in typeInfo.DeclaredFields)
            {
                try
                {
                    if (field.IsPublic && !field.IsStatic)
                    {
                        AddKnownTypesByType(field.FieldType, types);
                    }
                }
                catch { }
            }
            foreach (var property in typeInfo.DeclaredProperties)
            {
                try
                {
                    if (property.CanRead)
                    {
                        AddKnownTypesByType(property.PropertyType, types);
                    }
                }
                catch { }
            }
        }
    }
}
