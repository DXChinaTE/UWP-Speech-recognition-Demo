using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.Foundation;

namespace SpeechTTSDemo.Common
{
    public static class SpeechTTSHelper
    {

        private static SpeechRecognizer speechRecognizer = null;
        private static CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        private static IAsyncOperation<SpeechRecognitionResult> recognitionOperation = null;
        private static string _command = String.Empty;
        public static string _ErrMessage = "Sorry,Exception{} ocured";
      

        public  async static Task< string> RecognizeVoiceCommand()
        {
            try
            {             
                speechRecognizer = await ResourceHelper.InitRecognizer() ;
                if(null == speechRecognizer)
                {
                    _command = ResourceHelper.GetString("Sys Err");//"系统异常";
                    return _command;
                }
                
                recognitionOperation = speechRecognizer.RecognizeAsync();

                SpeechRecognitionResult speechRecognitionResult = await recognitionOperation;

                // If successful, display the recognition result. A cancelled task should do nothing.

                if (speechRecognitionResult.Status == SpeechRecognitionResultStatus.Success)
                {
                    if (speechRecognitionResult.Confidence == SpeechRecognitionConfidence.Rejected)
                    {
                        _command = ResourceHelper.GetString("invalid");//"对不起，无法识别您的命令";
                    }
                    else
                    {
                        string tag = "unknown";
                        if (speechRecognitionResult.Constraint != null)
                        {
                            // Only attempt to retreive the tag if we didn't hit the garbage rule.
                            tag = speechRecognitionResult.Constraint.Tag;
                        }

                        _command = speechRecognitionResult.Text;
                    }
                }
                return _command;
            }
            catch (Exception e)
            {
                return e.Message;
            }                     
        }
    }
}
