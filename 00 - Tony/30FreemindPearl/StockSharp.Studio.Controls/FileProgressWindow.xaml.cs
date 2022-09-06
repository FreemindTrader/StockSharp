﻿// Decompiled with JetBrains decompiler
// Type: StockSharp.Studio.Controls.FileProgressWindow
// Assembly: StockSharp.Studio.Controls, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D457EB08-5750-4F4B-A104-96BE70F84CCF
// Assembly location: T:\00-StockSharp\Designer\StockSharp.Studio.Controls.dll

using DevExpress.Xpf.Core;
using Ecng.Common;
using Ecng.Xaml;
using StockSharp.Localization;
using StockSharp.Logging;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Threading;

namespace StockSharp.Studio.Controls
{
    public partial class FileProgressWindow : ThemedWindow, IComponentConnector
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isProcessing;
        
        public FileProgressWindow()
        {
            this.InitializeComponent();
        }

        public (string name, byte[ ] body , long id ) File { get; set; }
        
        
        public event Func<string, byte[ ], Action<long>, CancellationToken, Task<StockSharp.Web.DomainModel.File>> FileProcessing;

        private void UpdateProgressBar( long current )
        {
            int total = this.File.Item2.Length;
            long per = current * 100L / ( long )total;
            ( ( DispatcherObject )this ).GuiSync<string>( ( Func<string> )( () => this.ProgressText.Text = string.Format( "{0}/{1} ({2}%)", ( object )current, ( object )total, ( object )per ) ) );
        }

        private void Cancel_OnClick( object sender, RoutedEventArgs e )
        {
            this.TryCancel();
        }

        private void TryCancel()
        {
            if ( new MessageBoxBuilder().Caption( this.Title ).Question().Text( LocalizedStrings.CancelOperationQuestion ).YesNo().Owner( ( Window )this ).Show() != MessageBoxResult.Yes )
                return;
            this._cts.Cancel();
        }

        private void FileProgressWindow_OnClosing( object sender, CancelEventArgs e )
        {
            e.Cancel = this._isProcessing;
            if ( !this._isProcessing )
                return;
            this.TryCancel();
        }

        private void FileProgressWindow_OnLoaded( object sender, RoutedEventArgs e )
        {
            ValueTuple<string, byte[ ], long> file = this.File;
            Func<string, byte[ ], Action<long>, CancellationToken, Task<StockSharp.Web.DomainModel.File>> evt = this.FileProcessing;
            ValueTuple<string, byte[ ], long> valueTuple = file;
            string str = valueTuple.Item1;
            byte[ ] numArray = valueTuple.Item2;
            long num = valueTuple.Item3;
            if ( str == ( string )null && numArray == null && num == 0L || evt == null )
                return;
            this.Progress.Value = 0.0;
            this.ProgressText.Text = string.Format( "0/{0}", ( object )file.Item2.Length );
            this.ProcessAction( ( Func<CancellationToken, Task<StockSharp.Web.DomainModel.File>> )( token => evt( file.Item1, file.Item2, new Action<long>( this.UpdateProgressBar ), token ) ) );
        }

        private void ProcessAction( Func<CancellationToken, Task<StockSharp.Web.DomainModel.File>> action )
        {
            if ( action == null )
                throw new ArgumentNullException( nameof( action ) );
            this._isProcessing = true;
            //( ( Action )( async () =>
            //   {
            //       try
            //       {
            //           Exception error = ( Exception )null;
            //           try
            //           {
            //               TaskAwaiter<StockSharp.Web.DomainModel.File> awaiter = action( this._cts.Token ).GetAwaiter();
            //               if ( !awaiter.IsCompleted )
            //               {
            //                   int num;
            //                   // ISSUE: explicit reference operation
            //                   // ISSUE: reference to a compiler-generated field
            //                   ( ^this ).\u003C\u003E1__state = num = 0;
            //                   TaskAwaiter<StockSharp.Web.DomainModel.File> taskAwaiter = awaiter;
            //                   // ISSUE: explicit reference operation
            //                   // ISSUE: reference to a compiler-generated field
            //                   ( ^this ).\u003C\u003Et__builder.AwaitUnsafeOnCompleted < TaskAwaiter<StockSharp.Web.DomainModel.File>, FileProgressWindow.\u003C\u003Ec__DisplayClass15_0.\u003C\u003CProcessAction\u003Eb__0\u003Ed > (ref awaiter, this);
            //                   return;
            //               }
            //               StockSharp.Web.DomainModel.File result = awaiter.GetResult();
            //               ValueTuple<string, byte[ ], long> file = this.File;
            //               file.Item3 = result.Id;
            //               this.File = file;
            //           }
            //           catch ( Exception ex )
            //           {
            //               if ( !( ex is OperationCanceledException ) )
            //               {
            //                   ex.LogError( ( string )null );
            //                   error = ex;
            //               }
            //           }
            //           this._isProcessing = false;
            //           ( ( DispatcherObject )this ).GuiSync( ( Action )( () =>
            //        {
            //            if ( error != null )
            //            {
            //                int num = ( int )new MessageBoxBuilder().Caption( this.Title ).Error().Text( error.Message ).Owner( ( Window )this ).Show();
            //            }
            //            this.DialogResult = new bool?( error == null && !this._cts.IsCancellationRequested );
            //        } ) );
            //       }
            //       catch ( Exception ex )
            //       {
            //           // ISSUE: explicit reference operation
            //           // ISSUE: reference to a compiler-generated field
            //           ( ^this ).\u003C\u003E1__state = -2;
            //           // ISSUE: explicit reference operation
            //           // ISSUE: reference to a compiler-generated field
            //           ( ^this ).\u003C\u003Et__builder.SetException( ex );
            //           return;
            //       }
            //     // ISSUE: explicit reference operation
            //     // ISSUE: reference to a compiler-generated field
            //     ( ^this ).\u003C\u003E1__state = -2;
            //       // ISSUE: explicit reference operation
            //       // ISSUE: reference to a compiler-generated field
            //       ( ^this ).\u003C\u003Et__builder.SetResult();
            //   } ) ).Thread().Launch();
        }

        
    }
}
