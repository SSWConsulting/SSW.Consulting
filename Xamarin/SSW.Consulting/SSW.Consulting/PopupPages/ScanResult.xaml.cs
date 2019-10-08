﻿using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using SSW.Consulting.Models;
using SSW.Consulting.ViewModels;
using Xamarin.Forms;

namespace SSW.Consulting.PopupPages
{
    public partial class ScanResult : PopupPage
    {
        public ScanResult(ChallengeResult result)
        {
            InitializeComponent();
            ScanResultViewModel viewModel = new ScanResultViewModel(result);
            viewModel.Navigation = Navigation;
            BindingContext = viewModel;
        }
    }
}
