﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;


namespace Umini.Test.mgh3326
{
    /// <summary>
    /// Interaction logic for Test_PlaylistParsing.xaml
    /// </summary>
    public partial class Test_PlaylistParsing : Window
    {
        public Test_PlaylistParsing()
        {
            InitializeComponent();
        }
    }
}
