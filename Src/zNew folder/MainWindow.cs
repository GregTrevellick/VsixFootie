﻿using FootieData.Common.Helpers;
using FootieData.Entities;
using FootieData.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FootballDataSDK.Services;

namespace HierarchicalDataTemplate
{
    public partial class MainWindow : Window
    {
        private static FootballDataSdkGateway _gateway;
        private static WpfHelper _wpfHelper;
        private static GeneralOptions _generalOptions;
        private static List<DataGridStanding2> dataGridStanding2s;
        private static List<DataGridResult2> dataGridResult2s;
        private static List<DataGridFixture2> dataGridFixture2s;

        private static List<Standing> dataGridItemsSourceStanding;
        private static List<Fixture> dataGridItemsSourceResult;
        private static List<Fixture> dataGridItemsSourceFixture;
        private static string _parentExpanderHeader;

        public MainWindow()
        {
            InitializeComponent();

            var _footDataServices = new FootDataServices
            {
                AuthToken = "52109775b1584a93854ca187690ed4bb"
            };

            _gateway = new FootballDataSdkGateway(_footDataServices);

            _wpfHelper = new WpfHelper();

            var tempryGetOptions = new TempryGetOptions();
            _generalOptions = tempryGetOptions.GetGeneralOptions();

            dataGridFixture2s = new List<DataGridFixture2>();
            dataGridResult2s = new List<DataGridResult2>();
            dataGridStanding2s = new List<DataGridStanding2>();

            //var leagueSubOptionsToShow = _generalOptions.LeagueOptions.Where(x => x.ShowLeague);

            //foreach (var subOptionsToShow in leagueSubOptionsToShow)
            //{
            //    var internalToExternalMappingExists = LeagueCodeMappings.Mappings.TryGetValue(subOptionsToShow.InternalLeagueCode, out ExternalLeagueCode externalLeagueCode);

            //    if (internalToExternalMappingExists)
            //    {
            //        foreach (var subOption in subOptionsToShow.LeagueSubOptions)
            //        {
            //            if (subOption.Expand)
            //            {
            //                LoadLeagueToShow(externalLeagueCode, subOption.GridType);
            //            }
            //        }
            //    }                
            //}
        }

        //private static async void LoadLeagueToShow(ExternalLeagueCode externalLeagueCode, GridType gridType)
        //{
        //    await Task.Run(() => LoadShownData(externalLeagueCode, gridType));
        //}
        private static void LoadLeagueToShow(ExternalLeagueCode externalLeagueCode, GridType gridType)
        {
            LoadShownData(externalLeagueCode, gridType);
        }

        //private static async void LoadShownData(ExternalLeagueCode externalLeagueCode, GridType gridType)
        private static void LoadShownData(ExternalLeagueCode externalLeagueCode, GridType gridType)
        {
            if (gridType == GridType.Standing)
            {
                //var leagueResponse = await _gateway.GetLeagueResponse_Standings(externalLeagueCode.ToString());
                var leagueResponse = _gateway.GetLeagueResponse_Standings(externalLeagueCode.ToString());
                dataGridStanding2s.Clear();
                dataGridStanding2s.Add(new DataGridStanding2
                {
                    ExternalLeagueCode = externalLeagueCode,
                    Standings = (List<Standing>) leagueResponse.Standings
                });
            }

            if (gridType == GridType.Result)
            {
                //var leagueMatchesResults = await _gateway.GetLeagueResponse_Results(externalLeagueCode.ToString());                
                var leagueMatchesResults = _gateway.GetLeagueResponse_Results(externalLeagueCode.ToString());
                dataGridResult2s.Add(new DataGridResult2
                {
                    ExternalLeagueCode = externalLeagueCode,
                    Results = (List<Fixture>)leagueMatchesResults.MatchFixtures
                });
            }

            if (gridType == GridType.Fixture)
            {
                //var leagueMatchesFixtures = await _gateway.GetLeagueResponse_Fixtures(externalLeagueCode.ToString());
                var leagueMatchesFixtures = _gateway.GetLeagueResponse_Fixtures(externalLeagueCode.ToString());
                dataGridFixture2s.Add(new DataGridFixture2
                {
                    ExternalLeagueCode = externalLeagueCode,
                    Fixtures = (List<Fixture>)leagueMatchesFixtures.MatchFixtures
                });
            }
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

        #region button
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await LoginAsync();
                //next line wont run til LoginAsync has finished
                MyBtn.Content = result;

            }
            catch (Exception)
            {
                MyBtn.Content = "internal error";
            }
        }

        private async Task<string> LoginAsync()
        {
            try
            {
                //the next 3 run at same time 
                var loginTask = Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    return "login successful";
                });

                var payTask = Task.Delay(2000);

                var purchaseTask = Task.Delay(1000);

                //next line ensures that only when all 3 are done do we return 
                await Task.WhenAll(loginTask, payTask, purchaseTask);

                //all 3 done before this line runs
                return loginTask.Result;
            }
            catch (Exception)
            {
                return "login failed";
            }
        }
        #endregion

        #region new DataGridLoaded_Any
        private async void DataGridLoaded_Any(object sender, RoutedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            Expander parentExpander = dataGrid.Parent as Expander;
            parentExpander.IsExpanded = true;
            #region grid properties
            var color = (Color)ColorConverter.ConvertFromString("#FFFFF0");
            dataGrid.AlternatingRowBackground = new SolidColorBrush(color);
            dataGrid.ColumnHeaderHeight = 2;
            dataGrid.RowHeaderWidth = 2;
            dataGrid.CanUserAddRows = false;
            dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            #endregion
            var gridType = _wpfHelper.GetGridType(dataGrid.Name);
            var parentExpanderName = parentExpander.Name;

            try
            {
                var result = await PopulateDataGridAsync(parentExpanderName, gridType);
                //next lines wont run til PopulateDataGridAsync above has finished
                switch (gridType)
                {
                    case GridType.Unknown:
                        break;
                    case GridType.Standing:
                        dataGrid.ItemsSource = dataGridItemsSourceStanding;
                        break;
                    case GridType.Result:
                        dataGrid.ItemsSource = dataGridItemsSourceResult;
                        break;
                    case GridType.Fixture:
                        dataGrid.ItemsSource = dataGridItemsSourceFixture;
                        break;
                }
                MyBtn.Content = result;
                parentExpander.Header = _parentExpanderHeader;
            }
            catch (Exception)
            {
                MyBtn.Content = "DataGridLoaded_Any internal error";
            }
        }

        private async Task<string> PopulateDataGridAsync(string parentExpanderName, GridType gridType)
        {
            try
            {
                //the next 3 run at same time 
                var standingsTask = Task.Run(() =>
                {
                    _parentExpanderHeader = GetAndPopulateDataGrid(parentExpanderName, gridType);
                    return "standings task done";
                });
                var resultsTask = Task.Delay(2);
                var fixturesTask = Task.Delay(1);

                //next line ensures that only when all 3 are done do we return 
                await Task.WhenAll(standingsTask, resultsTask, fixturesTask);

                //all 3 done before these line runs
                var r = new Random();
                var rv = r.Next();
                return standingsTask.Result + rv; 
            }
            catch (Exception)
            {
                return "login failed";
            }
        }

        private static string GetAndPopulateDataGrid(string parentExpanderName, GridType gridType)
        {
            var parentExpanderHeader = "parentExpanderHeader";
            var internalLeagueCode = InternalLeagueCode(parentExpanderName);
            var shouldShowLeague = ShouldShowLeague(internalLeagueCode);

            if (shouldShowLeague)
            {
                parentExpanderHeader = internalLeagueCode.GetDescription() + " " + gridType.GetDescription();

                if (ShouldExpandGrid(internalLeagueCode, gridType))
                {
                    var internalToExternalMappingExists = LeagueCodeMappings.Mappings.TryGetValue(internalLeagueCode, out ExternalLeagueCode externalLeagueCode);
                    if (internalToExternalMappingExists)
                    {
                        switch (gridType)
                        {
                            case GridType.Unknown:
                                break;
                            case GridType.Standing:
                                dataGridItemsSourceStanding = GetLeagueDataStanding(externalLeagueCode, gridType);
                                break;
                            case GridType.Result:
                                dataGridItemsSourceResult = GetLeagueDataResult(externalLeagueCode, gridType);
                                break;
                            case GridType.Fixture:
                                dataGridItemsSourceFixture = GetLeagueDataFixture(externalLeagueCode, gridType);
                                break;
                        }
                        //parentExpander.IsExpanded = true;
                    }
                    else
                    {
                        //TODO ERROR
                        //parentExpander.IsExpanded = false;
                    }
                }
                else
                {
                    //parentExpander.IsExpanded = false;
                }
            }
            else
            {
                //parentExpander.Visibility = Visibility.Collapsed;
            }

            return parentExpanderHeader;
        }

        private static List<Standing> GetLeagueDataStanding(ExternalLeagueCode externalLeagueCode, GridType gridType)
        {
            if (!dataGridStanding2s.Any(x => x.ExternalLeagueCode == externalLeagueCode && x.Standings.Count > 0))
            {
                LoadLeagueToShow(externalLeagueCode, gridType);
                var dataGridItemsSource = dataGridStanding2s
                    ?.Where(x => x.ExternalLeagueCode == externalLeagueCode)
                    .Select(x => x.Standings);
                return dataGridItemsSource.First();
            }
            return null;
        }

        private static List<Fixture> GetLeagueDataResult(ExternalLeagueCode externalLeagueCode, GridType gridType)
        {
            if (!dataGridResult2s.Any(x => x.ExternalLeagueCode == externalLeagueCode && x.Results.Count > 0))
            {
                LoadLeagueToShow(externalLeagueCode, gridType);
                var dataGridItemsSource = dataGridResult2s
                    ?.Where(x => x.ExternalLeagueCode == externalLeagueCode)
                    .Select(x => x.Results);
                return dataGridItemsSource.First();
            }
            return null;
        }

        private static List<Fixture> GetLeagueDataFixture(ExternalLeagueCode externalLeagueCode, GridType gridType)
        {
            if (!dataGridFixture2s.Any(x => x.ExternalLeagueCode == externalLeagueCode && x.Fixtures.Count > 0))
            {
                LoadLeagueToShow(externalLeagueCode, gridType);
                var dataGridItemsSource = dataGridFixture2s
                    ?.Where(x => x.ExternalLeagueCode == externalLeagueCode)
                    .Select(x => x.Fixtures);
                return dataGridItemsSource.First();
            }
            return null;
        }

        private static bool ShouldShowLeague(InternalLeagueCode internalLeagueCode)
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

        private static bool ShouldExpandGrid(InternalLeagueCode internalLeagueCode, GridType gridType)
        {
            return true;
            //if (_generalOptions.LeagueOptions.Any(x => x.InternalLeagueCode == internalLeagueCode
            //                                           && x.ShowLeague
            //                                           && x.LeagueSubOptions.Any(ccc => ccc.GridType == gridType
            //                                                                            && ccc.Expand)))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
        #endregion

        //static async void PopulateDataGrid1Async(object sender)
        //{
        //    // This method runs asynchronously.
        //    await Task.Run(() => PopulateDataGrid2Async(sender));
        //}

        //private static async void PopulateDataGrid2Async(object sender)
        //private static void PopulateDataGrid2Async(object sender)
        //private void GetAndPopulateDataGrid(object sender)
        //{
        //    var dataGrid = sender as DataGrid;
        //    Expander parentExpander = dataGrid.Parent as Expander;
        //    var internalLeagueCode = InternalLeagueCode(parentExpander.Name);
        //    var shouldShowLeague = ShouldShowLeague(internalLeagueCode);

        //    if (shouldShowLeague)
        //    {
        //        var gridType = _wpfHelper.GetGridType(dataGrid.Name);

        //        parentExpander.Header= internalLeagueCode.GetDescription() + " " + gridType.GetDescription();

        //        if (ShouldExpandGrid(internalLeagueCode, gridType))
        //        {
        //            var color = (Color)ColorConverter.ConvertFromString("#FFFFF0");
        //            dataGrid.AlternatingRowBackground = new SolidColorBrush(color);
        //            dataGrid.ColumnHeaderHeight = 2;
        //            dataGrid.RowHeaderWidth = 2;
        //            dataGrid.CanUserAddRows = false;
        //            dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;

        //            var internalToExternalMappingExists = LeagueCodeMappings.Mappings.TryGetValue(internalLeagueCode, out ExternalLeagueCode externalLeagueCode);
        //            if (internalToExternalMappingExists)
        //            {
        //                GetLeagueData(dataGrid, externalLeagueCode, gridType);
        //                parentExpander.IsExpanded = true;
        //            }
        //            else
        //            {
        //                //TODO ERROR
        //                parentExpander.IsExpanded = false;
        //            }
        //        }
        //        else
        //        {
        //            parentExpander.IsExpanded = false;
        //        }
        //    }
        //    else
        //    {
        //        parentExpander.Visibility = Visibility.Collapsed;
        //    }
        //}

        //private static bool ShouldShowLeague(InternalLeagueCode internalLeagueCode)
        //{
        //    if (_generalOptions.LeagueOptions.Any(x => x.InternalLeagueCode == internalLeagueCode && x.ShowLeague))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private static bool ShouldExpandGrid(InternalLeagueCode internalLeagueCode, GridType gridType)
        //{
        //    if (_generalOptions.LeagueOptions.Any(x => x.InternalLeagueCode == internalLeagueCode
        //                                               && x.ShowLeague
        //                                               && x.LeagueSubOptions.Any(ccc => ccc.GridType == gridType
        //                                                                                && ccc.Expand)))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private static async void GetLeagueData(DataGrid dataGrid, ExternalLeagueCode externalLeagueCode, GridType gridType)
        ////private static void GetLeagueData(DataGrid dataGrid, ExternalLeagueCode externalLeagueCode, GridType gridType)
        //{
        //    if (gridType == GridType.Standing)
        //    {
        //        //var leagueResponse = await _gateway.GetLeagueResponse_Standings(externalLeagueCode.ToString());
        //        //dataGrid.ItemsSource = leagueResponse.Standings;

        //        if (dataGridStanding2s.Any(x => x.ExternalLeagueCode == externalLeagueCode &&
        //                                        x.Standings.Count > 0))
        //        {
        //            //dataGridStanding2s is already populated so do nowt, not even setting the data grid source (as that will already have happened)
        //        }
        //        else
        //        {
        //            //dataGridStanding2s is not populated for this league
        //            LoadLeagueToShow(externalLeagueCode, gridType);
        //            var dataGridItemsSource = dataGridStanding2s
        //                ?.Where(x => x.ExternalLeagueCode == externalLeagueCode)
        //                .Select(x => x.Standings);
        //            dataGrid.ItemsSource = dataGridItemsSource.First();
        //        }
        //    }

        //    if (gridType == GridType.Result)
        //    {
        //        ////LeagueMatchesResults leagueMatchesResults = null;
        //        ////if (gridType == GridType.Result)
        //        ////{
        //        ////    leagueMatchesResults = await _gateway.GetLeagueResponse_Results(externalLeagueCode.ToString());
        //        ////}
        //        ////dataGrid.ItemsSource = leagueMatchesResults.MatchFixtures;
        //        //dataGrid.ItemsSource = dataGridResult2s?.Where(x => x.ExternalLeagueCode == externalLeagueCode).Select(x => x.Results); 

        //        if (dataGridResult2s.Any(x => x.ExternalLeagueCode == externalLeagueCode &&
        //                                        x.Results.Count > 0))
        //        {
        //            //dataGridResult2s is already populated so do nowt, not even setting the data grid source (as that will already have happened)
        //        }
        //        else
        //        {
        //            //dataGridResult2s is not populated for this league
        //            LoadLeagueToShow(externalLeagueCode, gridType);
        //            var dataGridItemsSource = dataGridResult2s
        //                ?.Where(x => x.ExternalLeagueCode == externalLeagueCode)
        //                .Select(x => x.Results);
        //            dataGrid.ItemsSource = dataGridItemsSource.First();
        //        }
        //    }

        //    if (gridType == GridType.Fixture)
        //    {
        //        ////LeagueMatchesFixtures leagueMatchesFixtures = null;
        //        ////if (gridType == GridType.Fixture)
        //        ////{
        //        ////    leagueMatchesFixtures = await _gateway.GetLeagueResponse_Fixtures(externalLeagueCode.ToString());
        //        ////}
        //        ////dataGrid.ItemsSource = leagueMatchesFixtures.MatchFixtures;
        //        //dataGrid.ItemsSource = dataGridFixture2s?.Where(x => x.ExternalLeagueCode == externalLeagueCode).Select(x => x.Fixtures);

        //        if (dataGridFixture2s.Any(x => x.ExternalLeagueCode == externalLeagueCode &&
        //                                      x.Fixtures.Count > 0))
        //        {
        //            //dataGridFixture2s is already populated so do nowt, not even setting the data grid source (as that will already have happened)
        //        }
        //        else
        //        {
        //            //dataGridFixture2s is not populated for this league
        //            LoadLeagueToShow(externalLeagueCode, gridType);
        //            var dataGridItemsSource = dataGridFixture2s
        //                ?.Where(x => x.ExternalLeagueCode == externalLeagueCode)
        //                .Select(x => x.Fixtures);
        //            dataGrid.ItemsSource = dataGridItemsSource.First();
        //        }
        //    }
        //}

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

        private static InternalLeagueCode InternalLeagueCode(string expanderName)
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

    public class DataGridStanding2
    {
        public ExternalLeagueCode ExternalLeagueCode { get; set; }
        public List<Standing> Standings { get; set; }
    }

    public class DataGridResult2
    {
        public ExternalLeagueCode ExternalLeagueCode { get; set; }
        public List<Fixture> Results { get; set; }
    }

    public class DataGridFixture2
    {
        public ExternalLeagueCode ExternalLeagueCode { get; set; }
        public List<Fixture> Fixtures { get; set; }
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
