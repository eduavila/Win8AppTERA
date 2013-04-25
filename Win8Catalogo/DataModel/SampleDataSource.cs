using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using Win8Catalogo.Catalogo.Model;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace Win8Catalogo.Data
{
    

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<Categoria> _allGroups = new ObservableCollection<Categoria>();
        public ObservableCollection<Categoria> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<Categoria> GetGroups(string uniqueId)
        {
            //if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _sampleDataSource.AllGroups;
        }

        public static Categoria GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.ID.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static Item GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.ID.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");

            var group1 = new Categoria("Group-1",
                    "Automodelo On Road",
                    "Velocidade sem limites",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante", 
                    "Assets/Items/Categoria2/Item1.jpg");
            group1.Items.Add(new Item("Group-1-Item-1",
                    "Lorem ipsum dolor",
                    ITEM_CONTENT,
                    "400.00",
                    "Assets/Items/Categoria1/Item1.jpg",
                    "1:10 Losi RTR",
                    "http://pt.wikipedia.org/wiki/Automodelos_off_road",
                    group1.ID));
            group1.Items.Add(new Item("Group-1-Item-2",
                    "Lorem ipsum dolor",
                    ITEM_CONTENT,
                    "480.00",
                    "Assets/Items/Categoria1/Item2.jpg",
                    "1:10 Losi RR",
                    "http://pt.wikipedia.org/wiki/Automodelos_off_road",
                    group1.ID));
            group1.Items.Add(new Item("Group-1-Item-3",
                    "Lorem ipsum dolor",
                    ITEM_CONTENT,
                    "400.00",
                    "Assets/Items/Categoria1/Item3.jpg",
                    "1:10 Traxxas",
                    "http://pt.wikipedia.org/wiki/Automodelos_off_road",
                    group1.ID));
            group1.Items.Add(new Item("Group-1-Item-4",
                    "Lorem ipsum dolor",
                    ITEM_CONTENT,
                    "900.00",
                    "Assets/Items/Categoria1/Item4.jpg",
                    "1:10 Losi XXXX",
                    "http://pt.wikipedia.org/wiki/Automodelos_off_road",
                    group1.ID));
            
            this.AllGroups.Add(group1);

            var group2 = new Categoria("Group-2",
                    "Automodelo OFF Road",
                    "Emoção fora das pistas",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante",
                    "Assets/Items/Categoria2/Item1.jpg");
            group2.Items.Add(new Item("Group-2-Item-1",
                    "Lorem ipsum dolor",
                    ITEM_CONTENT,
                    "400.00",
                    "Assets/Items/Categoria2/Item1.jpg",
                    "1:10 Losi RTR",
                    "http://pt.wikipedia.org/wiki/Automodelos_off_road",
                    group2.ID));
            group2.Items.Add(new Item("Group-2-Item-2",
                    "Lorem ipsum dolor",
                    ITEM_CONTENT,
                    "480.00",
                    "Assets/Items/Categoria2/Item2.jpg",
                    "1:10 Losi RR",
                    "http://pt.wikipedia.org/wiki/Automodelos_off_road",
                    group2.ID));
            group2.Items.Add(new Item("Group-2-Item-3",
                    "Lorem ipsum dolor",
                    ITEM_CONTENT,
                    "400.00",
                    "Assets/Items/Categoria2/Item3.jpg",
                    "1:10 Traxxas",
                    "http://pt.wikipedia.org/wiki/Automodelos_off_road",
                    group2.ID));
            group2.Items.Add(new Item("Group-2-Item-4",
                    "Lorem ipsum dolor",
                    ITEM_CONTENT,
                    "900.00",
                    "Assets/Items/Categoria2/Item4.jpg",
                    "1:10 Losi XXXX",
                    "http://pt.wikipedia.org/wiki/Automodelos_off_road",
                    group2.ID));

            this.AllGroups.Add(group2);



        }
    }
}
