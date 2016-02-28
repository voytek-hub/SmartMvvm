﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Ninject;
using SmartMvvm.ViewModels;
using SmartMvvm.ViewModels.Interfaces;

namespace SmartMvvm.UI.WPF
{
    public class NavigationService : INavigationService
    {
        private readonly Frame frame;
        private readonly Dictionary<string, IPageViewModel> pages = new Dictionary<string, IPageViewModel>();

        public NavigationService(Frame frame)
        {
            Debug.WriteLine(GetType().Name + " created.");
            this.frame = frame;
        }

        public void InjectPage(string pageKey, IPageViewModel viewModel)
        {
            pages.Add(pageKey, viewModel);
        }

        public void GoBack()
        {
            frame.GoBack();
        }

        public void GoForward()
        {
            frame.GoForward();
        }

        public bool Navigate(IPageViewModel pageViewModel)
        {
            string viewModel = pageViewModel.GetType().Name;
            string pageName = viewModel.Substring(0, viewModel.IndexOf("ViewModel", StringComparison.Ordinal));

            var type = Assembly.GetExecutingAssembly().GetTypes().SingleOrDefault(a => a.Name.Equals(pageName));
            if (type == null) return false;

            var src = Activator.CreateInstance(type, pageViewModel);
            return frame.Navigate(src);
        }

        public bool Navigate(string pageKey)
        {
            IPageViewModel viewModel;
            if (pages.TryGetValue(pageKey, out viewModel))
            {
                return Navigate(viewModel);
            }
            return false;
        }
    }
}