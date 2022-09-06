﻿using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Docking.Base;
using StockSharp.Algo;
using StockSharp.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using fx.Definitions;
using fx.Common;
using fx.Definitions.UndoRedo;
using fx.Algorithm;
using Ecng.Collections;
using StockSharp.Logging;
using fx.Indicators;
using DevExpress.Xpf.Bars;
using fx.Database;
using System.Data.Common;

namespace FreemindAITrade.ViewModels
{
    public partial class TradeStationViewModel : BaseLogReceiver, IMutltiTimeFrameSessionDataRepo
    {
        public void CopyBarTimeToClipboard()
        {
            _selectedViewModel.CopyBarTimeToClipboard();
        }        

        public void TestTTS()
        {
            //using ( SpeechSynthesizer synthesizer = new SpeechSynthesizer( ) )
            //{
            //    // show installed voices
            //    foreach ( var v in synthesizer.GetInstalledVoices( ).Select( v => v.VoiceInfo ) )
            //    {
            //        Console.WriteLine( "Name:{0}, Gender:{1}, Age:{2}",
            //          v.Description, v.Gender, v.Age );
            //    }

            //    // select male senior (if it exists)
            //    synthesizer.SelectVoiceByHints( VoiceGender.Female, VoiceAge.Teen );

            //    // select audio device
            //    synthesizer.SetOutputToDefaultAudioDevice( );

            //    // build and speak a prompt
            //    PromptBuilder builder = new PromptBuilder();
            //    builder.AppendText( "Wave A is now broken downward !" );
            //    synthesizer.Speak( builder );
            //}

            //SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            //synthesizer.Volume = 100;  // 0...100
            //synthesizer.Rate = -2;     // -10...10

            // Synchronous
            //synthesizer.Speak( "Wave A Broken Down" );

        }

        public void ConvertEWaveTable( )
        {
            var debugUndo = new UndoRedoArea( "Debug" );

            using ( debugUndo.Start( "ConvertEWaveTable" ) )
            {
                using ( var context = new ForexDatabars( ) )
                {
                    var allWaves = context.ELLIOTTWAVE;

                    foreach ( var wave in allWaves )
                    {
                        try
                        {
                            DateTimeOffset myDt = wave.StartDate.FromLinuxTime( );
                            wave.WaveDate = myDt;
                        }
                        catch ( DbException ex )
                        {
                            this.AddErrorLog( ex );
                        }
                    }

                    //if ( allWaves.Any( ) )
                    //{
                    //    var first = found.FirstOrDefault( );
                    //    first.StartDate = waveInfo.StartDate;
                    //    first.HarmonicElliottWaveBit = waveInfo.HarmonicElliottWaveBit;
                    //    first.HarmonicElliottWaveExtraBit = waveInfo.HarmonicElliottWaveExtraBit;
                    //    first.AlternativeHewBit = waveInfo.AlternativeHewBit;
                    //    first.AlternativeHewExtraBit = waveInfo.AlternativeHewExtraBit;
                    //    first.WaveLabelPosition = waveInfo.WaveLabelPosition;
                    //    first.OfferId = waveInfo.OfferId;
                    //    first.Period = waveInfo.Period;
                    //    first.HighestWaveCycle = waveInfo.HighestWaveCycle;
                    //}
                    //else
                    //{
                    //    var first = context.ELLIOTTWAVE.Create( );
                    //    first.StartDate = waveInfo.StartDate;
                    //    first.HarmonicElliottWaveBit = waveInfo.HarmonicElliottWaveBit;
                    //    first.HarmonicElliottWaveExtraBit = waveInfo.HarmonicElliottWaveExtraBit;
                    //    first.AlternativeHewBit = waveInfo.AlternativeHewBit;
                    //    first.AlternativeHewExtraBit = waveInfo.AlternativeHewExtraBit;
                    //    first.WaveLabelPosition = waveInfo.WaveLabelPosition;
                    //    first.OfferId = waveInfo.OfferId;
                    //    first.Period = waveInfo.Period;
                    //    first.HighestWaveCycle = waveInfo.HighestWaveCycle;

                    //    context.ELLIOTTWAVE.Add( first );
                    //}

                    //var waveList = GetSelectedRemovedWavesList( timeframe );

                    //if ( waveList.Count > 0 )
                    //{
                    //    didUpdate = true;

                    //    
                    //}

                    try
                    {
                        context.SaveChanges( );
                    }
                    catch ( DbException ex )
                    {
                        this.AddErrorLog( ex );
                    }

                    Messenger.Default.Send( new WorkDoneMessage( "Save Waves to Database" ) );
                }
            }
        }

        
    }
}
