﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using ScreenCaptureMVVM.Model;
using ScreenCaptureMVVM.View;
using Screen = System.Windows.Forms.Screen;

namespace ScreenCaptureMVVM.ViewModel
{
    public class AreaSelectViewModel : StateViewModel
    {
        // I realy am sorry for the length of this line...
        private static readonly ReadOnlyObservableCollection<Type> _capturers
            = new ReadOnlyObservableCollection<Type>(
              new ObservableCollection<Type>(
                  typeof(ScreenCapturer).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ScreenCapturer)))));

        public static ReadOnlyObservableCollection<Type> Capturers
        {
            get
            {
                return _capturers;
            }
        }

        private Type _selectedArea;

        public Type SelectedArea
        {
            get { return _selectedArea; }
            set
            {
                _selectedArea = value;
                RaisePropertyChanged("SelectedArea");
            }
        }

        public override UserControl NextView
        {
            get
            {
                var v = new ScreenCaptureControl();
                var viewModel = ((ScreenCaptureViewModel)v.DataContext);
                viewModel.ScreenCapturer = (ScreenCapturer)Activator.CreateInstance(SelectedArea, new object[] { Screen });
                return v;
            }
        }

        public override UserControl BackView
        {
            get { return new DescriptionControl(); }
        }


        public override bool NextEnabled
        {
            get { return SelectedArea != null; }
        }

        public override bool BackEnabled
        {
            get { return true; }
        }

        public override string NextMessage
        {
            get { return "NEXT"; }
        }

        public override string BackMessage
        {
            get { return "CANCEL"; }
        }

        public Screen Screen { get; set; }
    }
}
