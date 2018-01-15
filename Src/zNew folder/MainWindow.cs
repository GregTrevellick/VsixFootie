﻿using FootieData.Common.Helpers;
using FootieData.Entities;
using FootieData.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FootballDataSDK.Services;

namespace HierarchicalDataTemplate
{
    public partial class MainWindow : Window
    {
        private void AddThem(InternalLeagueCode internalLeagueCode)
        {
            _generalOptions.LeagueOptions.Add(
                new LeagueOption
                {
                    InternalLeagueCode = internalLeagueCode,
                    ShowLeague = true,
                    LeagueSubOptions = new List<LeagueSubOption>
                        {new LeagueSubOption {Expand = true, GridType = GridType.Standing}}
                });

            _generalOptions.LeagueOptions.Add(new LeagueOption
            {
                InternalLeagueCode = internalLeagueCode,
                ShowLeague = true,
                LeagueSubOptions = new List<LeagueSubOption>
                    {new LeagueSubOption {Expand = false, GridType = GridType.Result}}
            });

            _generalOptions.LeagueOptions.Add(new LeagueOption
            {
                InternalLeagueCode = internalLeagueCode,
                ShowLeague = true,
                LeagueSubOptions = new List<LeagueSubOption>
                    {new LeagueSubOption {Expand = false, GridType = GridType.Fixture}}
            });
        }

        private void GetGeneralOptions()
        {
            _generalOptions = new GeneralOptions
            {
                LeagueOptions = new List<LeagueOption>()
            };

            AddThem(HierarchicalDataTemplate.InternalLeagueCode.DE1);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.DE2);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.DE3);
            //AddThem(HierarchicalDataTemplate.InternalLeagueCode.DE4);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.ES1);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.ES3);
            //AddThem(HierarchicalDataTemplate.InternalLeagueCode.ES3);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.FR1);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.FR2);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.GR1);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.IT1);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.IT2);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.NL1);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.PT1);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.UK1);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.UK2);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.UK2);
            AddThem(HierarchicalDataTemplate.InternalLeagueCode.UK3);
            //AddThem(HierarchicalDataTemplate.InternalLeagueCode.UK4);
        }

        private readonly FootballDataSdkGateway _gateway;
        private readonly WpfHelper _wpfHelper;
        private GeneralOptions _generalOptions;

        public MainWindow()
        {
            InitializeComponent();

            var _footDataServices = new FootDataServices
            {
                AuthToken = "52109775b1584a93854ca187690ed4bb"
            };
            _gateway = new FootballDataSdkGateway(_footDataServices);

            _wpfHelper = new WpfHelper();

            GetGeneralOptions();
        }

        private void ExpanderLoaded_Any(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            var internalLeagueCode = InternalLeagueCode(expander.Name);
            var shouldShowLeague = ShouldShowLeague(internalLeagueCode);
            if (shouldShowLeague)
            {
                expander.Visibility = Visibility.Visible;
                expander.Style = (Style)TryFindResource("PlusMinusExpander");
            }
            else
            {
                expander.Visibility = Visibility.Collapsed;
            }
        }

        public static object TryFindResource(FrameworkElement element, object resourceKey)//gregt make private ?
        {
            var currentElement = element;

            while (currentElement != null)
            {
                var resource = currentElement.Resources[resourceKey];
                if (resource != null)
                {
                    return resource;
                }

                currentElement = currentElement.Parent as FrameworkElement;
            }

            return Application.Current.Resources[resourceKey];
        }

        private void DataGridLoaded_Any(object sender, RoutedEventArgs e)
        {
            PopulateDataGrid(sender);
        }

        private void PopulateDataGrid(object sender)
        {
            var dataGrid = sender as DataGrid;
            Expander parentExpander = dataGrid.Parent as Expander;
            var internalLeagueCode = InternalLeagueCode(parentExpander.Name);
            var shouldShowLeague = ShouldShowLeague(internalLeagueCode);

            if (shouldShowLeague)
            {
                var gridType = _wpfHelper.GetGridType(dataGrid.Name);

                parentExpander.Header= internalLeagueCode.GetDescription() + " " + gridType.GetDescription();

                if (ShouldExpandGrid(internalLeagueCode, gridType))
                {
                    var color = (Color)ColorConverter.ConvertFromString("#FFFFF0");
                    dataGrid.AlternatingRowBackground = new SolidColorBrush(color);
                    dataGrid.ColumnHeaderHeight = 2;
                    dataGrid.RowHeaderWidth = 2;
                    dataGrid.CanUserAddRows = false;
                    dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;

                    var internalToExternalMappingExists = LeagueCodeMappings.Mappings.TryGetValue(internalLeagueCode, out ExternalLeagueCode externalLeagueCode);
                    if (internalToExternalMappingExists)
                    {
                        GetLeagueData(dataGrid, externalLeagueCode, gridType);
                        parentExpander.IsExpanded = true;
                    }
                    else
                    {
                        //TODO ERROR
                        parentExpander.IsExpanded = false;
                    }
                }
                else
                {
                    parentExpander.IsExpanded = false;
                }
            }
            else
            {
                parentExpander.Visibility = Visibility.Collapsed;
            }
        }

        private bool ShouldShowLeague(InternalLeagueCode internalLeagueCode)
        {
            if (_generalOptions.LeagueOptions.Any(x => x.InternalLeagueCode == internalLeagueCode && x.ShowLeague))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ShouldExpandGrid(InternalLeagueCode internalLeagueCode, GridType gridType)
        {
            if (_generalOptions.LeagueOptions.Any(x => x.InternalLeagueCode == internalLeagueCode
                                                       && x.ShowLeague
                                                       && x.LeagueSubOptions.Any(ccc => ccc.GridType == gridType
                                                                                        && ccc.Expand)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void GetLeagueData(DataGrid dataGrid, ExternalLeagueCode externalLeagueCode, GridType gridType)
        {
            if (gridType == GridType.Standing)
            {
                var leagueResponse = await _gateway.GetLeagueResponse_Standings(externalLeagueCode.ToString());
                dataGrid.ItemsSource = leagueResponse.Standings;
            }

            if (gridType == GridType.Result)
            {
                LeagueMatchesResults leagueMatchesResults = null;

                if (gridType == GridType.Result)
                {
                    leagueMatchesResults = await _gateway.GetLeagueResponse_Results(externalLeagueCode.ToString());
                }
                dataGrid.ItemsSource = leagueMatchesResults.MatchFixtures;

            }

            if (gridType == GridType.Fixture)
            { 
                LeagueMatchesFixtures leagueMatchesFixtures = null;

                if (gridType == GridType.Fixture)
                {
                    leagueMatchesFixtures = await _gateway.GetLeagueResponse_Fixtures(externalLeagueCode.ToString());
                }

                dataGrid.ItemsSource = leagueMatchesFixtures.MatchFixtures;
            }
        }

        private void Click_Handler1(object sender, RoutedEventArgs e)
        {
            TextBlockBossMode.Text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Sed aliquam, libero eget vehicula aliquam, metus magna rhoncus lectus, ut malesuada tellus felis et nunc.Curabitur at sodales tortor, non tincidunt nisi. "
                                     + Environment.NewLine + 
                                     @"Quisque auctor bibendum metus et suscipit. Mauris sit amet metus interdum, faucibus metus et, placerat tellus. Suspendisse maximus dui dolor, vel vestibulum nisi porta sit amet.Nulla maximus dui et nisi gravida laoreet.Suspendisse sed tempor mi."
                                     + Environment.NewLine
                                     + @"Curabitur sit amet posuere felis, non sagittis sem.Vivamus pellentesque mi sapien, id elementum diam dictum in."
                                     + Environment.NewLine + 
                                     @"Nunc ut neque finibus, rutrum diam et, congue eros.Nulla ut metus sit amet tortor finibus mollis tempus eget nibh."
                                     + Environment.NewLine + 
                                     @"Mauris non rutrum nulla, volutpat eleifend leo. Pellentesque a iaculis est, at volutpat mi. Vestibulum ullamcorper dictum tincidunt. Cras ac enim vel orci accumsan tristique sed mattis ex."
                                     + Environment.NewLine + 
                                     @"Aliquam erat volutpat.Aenean ut sem nec leo molestie pharetra.Aenean velit ipsum, cursus eget nisl eget, facilisis vehicula nibh. "
                                     + Environment.NewLine + 
                                     @"Aliquam et metus ornare ante ullamcorper consectetur.Quisque sollicitudin sapien nulla, a mollis ante pellentesque ut. Aliquam erat volutpat.Maecenas condimentum iaculis lobortis. Vivamus non facilisis tortor."
                                     + Environment.NewLine+
                                     @"Etiam in viverra purus. Nullam viverra fringilla lacus. Nam laoreet arcu id bibendum accumsan. Curabitur semper quam nisi, ultricies suscipit nibh laoreet nec. " 
                                     + Environment.NewLine +
                                     @"In turpis metus, venenatis sit amet turpis vel, gravida maximus arcu.Etiam a elit ante. Donec quis odio erat. Aenean vel est quis ligula mattis tristique et at sem. Nulla malesuada, ante vel hendrerit fringilla, diam augue pulvinar nunc, eget consectetur felis orci eu sem.Vestibulum id laoreet ex. "
                                     + Environment.NewLine + 
                                     @"Pellentesque libero dolor, interdum nec urna at, convallis vehicula purus. Donec elementum mi nulla, a maximus tortor rhoncus vitae. Quisque pellentesque eros nibh. Cras metus velit, aliquet ut volutpat non, eleifend at dolor."
                                     + Environment.NewLine+
                                     @"Proin eget sodales mi. Donec volutpat vitae lectus ut efficitur. Integer efficitur eu lorem at tincidunt. Mauris id magna dictum, vulputate turpis sed, euismod enim.Nulla commodo tincidunt blandit."
                                     + Environment.NewLine + 
                                     @"Pellentesque laoreet justo sed porta dignissim. Quisque vitae erat eget lorem hendrerit semper scelerisque nec dui. Suspendisse vitae nisl ullamcorper nunc sollicitudin dictum ut quis tellus.";

          StackPanelLeagueMode.Visibility = Visibility.Collapsed;
            StackPanelBossMode.Visibility = Visibility.Visible;
        }

        private void Click_Handler2(object sender, RoutedEventArgs e)
        {
            StackPanelLeagueMode.Visibility = Visibility.Visible;
            StackPanelBossMode.Visibility = Visibility.Collapsed;
        }

        private InternalLeagueCode InternalLeagueCode(string expanderName)
        {
            var internalLeagueCodeString = _wpfHelper.GetInternalLeagueCodeString(expanderName);
            var internalLeagueCode = GetInternalLeagueCode(internalLeagueCodeString);
            return internalLeagueCode;
        }

        private static InternalLeagueCode GetInternalLeagueCode(string internalLeagueCodeString)
        {
            var internalLeagueCode = (InternalLeagueCode)Enum.Parse(typeof(InternalLeagueCode), internalLeagueCodeString);
            return internalLeagueCode;
        }
    }
}

////////////private void ContextMenuOpening_Any(object sender, ContextMenuEventArgs e)
////////////{
////////////    var expander = sender as Expander;

////////////    //var style = new Style();
////////////    //style.Resources = new ResourceDictionary();
////////////    //style.Resources.Add("StaticResource", "PlusMinusExpander");
////////////    //expander.SetValue(StyleProperty, style);

////////////    foreach (MyDataGrid myDataGrid in FindVisualChildren<MyDataGrid>(expander))
////////////    {
////////////        PopulateDataGrid(myDataGrid);
////////////    }
////////////}

//private void ExpanderExpanded_Any(object sender, RoutedEventArgs e)
//{
//    //    var expander = sender as Expander;

//    //    //var style = new Style();
//    //    //style.Resources = new ResourceDictionary();
//    //    //style.Resources.Add("StaticResource", "PlusMinusExpander");
//    //    //expander.SetValue(StyleProperty, style);

//    //    var kids = FindVisualChildren<MyDataGrid>(expander);

//    //    var kidsCount = kids.Count();
//    //    //if (kids.Count() == 0)
//    //    //{
//    //    //    expander.UpdateLayout();
//    //    //    foreach (MyDataGrid myDataGrid in kids)
//    //    //    {
//    //    //        PopulateDataGrid(myDataGrid);
//    //    //    }
//    //    //}
//    //    //else
//    //    //{
//    //        foreach (MyDataGrid myDataGrid in kids)
//    //        {
//    //            PopulateDataGrid(myDataGrid);
//    //        }
//    //    //}
//}

//public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
//{
//    if (depObj != null)
//    {
//        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
//        {
//            DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
//            if (child != null && child is T)
//            {
//                yield return (T)child;
//            }

//            foreach (T childOfChild in FindVisualChildren<T>(child))
//            {
//                yield return childOfChild;
//            }
//        }
//    }
//}
