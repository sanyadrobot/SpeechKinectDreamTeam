using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionEngine : MonoBehaviour
{


   

    public AudioClip[] music;
    public string[] weathertoday;
    public string[] weathertomorrow;
    public string[] weatheron;
    public string[] turnonmusic;
    public string[] turnoffmusic;
    public string[] opengoogle;
    public GameObject lefthand;
    public float musicvolumespeed = 0.01f;
    public Text txt;
    int current_clip = 0;

    private AudioSource source;

    float timer_change_music = 0.5f;

    float startposition = 0;
    float endposition = 0;

    string result = "";
    string complete_result="";
    private DictationRecognizer m_DictationRecognizer;

    float timer = 0f;




    void Start()
    {
        
        source = GetComponent<AudioSource>();
        
        m_DictationRecognizer = new DictationRecognizer();
        m_DictationRecognizer.AutoSilenceTimeoutSeconds = 6000000f;
        m_DictationRecognizer.InitialSilenceTimeoutSeconds = 9999999f;
        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            if(result != "")
            {
                result = text;
                complete_result = result;
            }            

        };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {

            Debug.LogFormat("Dictation hypothesis: {0}", text);
            result = text;
            txt.text += text;
            timer = 3;
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            //if (completionCause != DictationCompletionCause.Complete)
            //    Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
            
        };
        
        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        m_DictationRecognizer.Start();
    }
    private void Update()
    {
        if(timer < 2)
        {
            m_DictationRecognizer.Dispose();
            m_DictationRecognizer = new DictationRecognizer();
            m_DictationRecognizer.AutoSilenceTimeoutSeconds = 6000000f;
            m_DictationRecognizer.InitialSilenceTimeoutSeconds = 9999999f;
            m_DictationRecognizer.DictationResult += (text, confidence) =>
            {
                Debug.LogFormat("Dictation result: {0}", text);
                if (result != "")
                {
                    result = text;
                    complete_result = result;
                }

            };

            m_DictationRecognizer.DictationHypothesis += (text) =>
            {

                Debug.LogFormat("Dictation hypothesis: {0}", text);
                result = text;
                txt.text += text;
                timer = 3;
            };

            m_DictationRecognizer.DictationComplete += (completionCause) =>
            {
                //if (completionCause != DictationCompletionCause.Complete)
                //    Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);

            };

            m_DictationRecognizer.DictationError += (error, hresult) =>
            {
                Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
            };

            m_DictationRecognizer.Start();
            timer = 7f;
        }


        
         
            timer = timer - Time.deltaTime;

        if (m_DictationRecognizer == null)
        {
            return;
        }

        if (m_DictationRecognizer.Status == SpeechSystemStatus.Failed)
        {
            m_DictationRecognizer.Start();
            Debug.Log("Fail");
        }
        else if(m_DictationRecognizer.Status == SpeechSystemStatus.Stopped)
        {
            m_DictationRecognizer.Start();
            Debug.Log("Stopped");
            
        }
        else
        {
            Debug.Log("Running");
        }
       
            foreach (string tmp in weathertoday)
                if (tmp == result)
                {
                    Application.OpenURL("https://sinoptik.ua/%D0%BF%D0%BE%D0%B3%D0%BE%D0%B4%D0%B0-%D0%BA%D0%B8%D0%B5%D0%B2/2018-06-05");
                    result = "";
                }
            foreach (string tmp in weathertomorrow)
                if (tmp == result)
                {
                    Application.OpenURL("https://sinoptik.ua/%D0%BF%D0%BE%D0%B3%D0%BE%D0%B4%D0%B0-%D0%BA%D0%B8%D0%B5%D0%B2/2018-06-06");
                    result = "";
                }
            foreach (string tmp in turnonmusic)
                if (tmp == result)
                {
                    current_clip = UnityEngine.Random.Range(0, music.Length);
                    source.clip = music[current_clip];
                    source.Play();
                    source.volume = 0.7f;
                    result = "";
                }
            foreach (string tmp in opengoogle)
                if (tmp == result)
                {
                    Application.OpenURL("https://www.google.com/");
                    result = "";
                }
            foreach (string tmp in turnoffmusic)
            {
                if (tmp == result)
                {
                    source.Stop();
                    result = "";
                    
                }
            }
            if (complete_result.Contains("football") || (timer < 0 && result.Contains("football")))
            {
                if (football(complete_result) != null)
                {
                    Application.OpenURL(football(complete_result));
                    result = "";
                    complete_result = "";
                }
                else if (football(result)!=null && timer < 0)
                {
                Application.OpenURL(football(result));
                result = "";
                complete_result = "";
            }
            }
        if (complete_result.Contains("google") || (timer < 0 && result.Contains("google")))
        {
            if (googlesearch(complete_result) != null || result.Contains("google"))
            {
                if(timer > 0)
                Application.OpenURL(googlesearch(complete_result));
                else
                Application.OpenURL(googlesearch(result));
                complete_result = "";
                result = "";
            }
        }


        if (source.isPlaying)
        {
            if(lefthand.transform.position.y > 6)
            {
                source.volume = source.volume + musicvolumespeed;

            }
            else if (lefthand.transform.position.y < -2f)
            {
                source.volume = source.volume - musicvolumespeed;

            }
            endposition = lefthand.transform.position.x;

            if(startposition - endposition >= 4f)
            {
                if(current_clip+1 < music.Length)
                {
                    current_clip++;
                }
                else
                {
                    current_clip = 0;
                }
                source.clip = music[current_clip];
                source.Play();
            }
            else if(startposition - endposition <= -4f)
            {
                if (current_clip - 1 < 0)
                {
                    current_clip = music.Length - 1;
                }
                else if(current_clip - 1 > 0)
                {
                    current_clip--;
                }
                source.clip = music[current_clip];
                source.Play();
            }

            if (timer_change_music <= 0)
            {
                startposition = lefthand.transform.position.x;
                timer_change_music = 0.5f;
            }
            timer_change_music = timer_change_music - Time.deltaTime;
        }
    }

    public static string googlesearch(string input)
    {
        if (input.Split(' ')[0] != "google")
        {
            return null;
        }

        string searchstring = input.Substring(input.IndexOf(' ') + 1);
        searchstring.Replace("google", "");
        searchstring.Replace(' ', '+');
        


        return "https://www.google.com.ua/search?q=" + searchstring;


    }


     public static string football(string input)
        {
            if (input.Split(' ')[0] != "football") {
                return null;
            }
            string dateStr = input.Substring(input.IndexOf(' ') + 1);
        System.DateTime date = default(DateTime);
            
            if (dateStr == "today") {
                date = System.DateTime.Today;
            }
            else if (dateStr == "tomorrow") {
                date = DateTime.Today.AddDays(1);
            }
            else if (!DateTime.TryParse(dateStr, out date)) {
                return null;
            }
            
            return "https://www.transfermarkt.com/live/index?datum=" + date.ToString("yyyy-MM-dd");
        }
}
