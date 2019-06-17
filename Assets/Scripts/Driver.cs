using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Driver : MonoBehaviour
{
    private string configPath = "";
    private string URL = "http://localhost:8000/predict_viseme?req=";
    public InputField input;
    public Text textArea;
    public GameObject head;
    public float blendSpeed = 25f;
    public float shapeInit = 75;
    //private AudioSource audio;
    public Button button;
    private SkinnedMeshRenderer keys;
    private bool toAnimate;
    //private bool toSpeak;
    // private string speech = "";
    private float shapekeySize = 0f;
    private List<int> shapekeys;
    private int prev;
    private float revLimit = 70f;

    private void Start()
    {
        URL = FileRead.FindPathFromConfig(configPath, "CHATBOT_SERVER_URL");
        toAnimate = false;
        //toSpeak = false;
        //audio = GetComponent<AudioSource>();
        keys = head.GetComponent<SkinnedMeshRenderer>();
        shapekeys = new List<int>();
    }
    private void Update()
    {
        if (toAnimate)
        {
            if (shapekeys.Count > 0)
            {
                if (shapekeys[0] == -2)
                {
                    if (shapekeySize > revLimit)
                    {
                        keys.SetBlendShapeWeight(prev, shapekeySize);
                        shapekeySize -= blendSpeed;
                    }
                    else
                    {
                        if (shapekeys.Count == 1)
                        {
                            shapekeySize = 0f;
                            keys.SetBlendShapeWeight(prev, shapekeySize);
                            shapekeySize = 100;
                            shapekeys.RemoveAt(0);
                        }
                        else
                        {
                            if (shapekeys[1] == -2)
                            {
                                revLimit -= 35;
                                shapekeys.RemoveAt(0);
                            }
                            else
                            {
                                revLimit = 70f;
                                shapekeySize = 0f;
                                keys.SetBlendShapeWeight(prev, shapekeySize);
                                shapekeySize = shapeInit;
                                shapekeys.RemoveAt(0);
                                if (shapekeys.Count > 0)
                                    keys.SetBlendShapeWeight(shapekeys[0], shapekeySize);

                            }
                        }
                    }

                }
                else
                {
                    if (shapekeys[0] == prev)
                    {
                        if (shapekeySize > 70f)
                        {
                            keys.SetBlendShapeWeight(prev, shapekeySize);
                            shapekeySize -= blendSpeed;
                        }
                        else
                        {
                            if (shapekeys[1] == -2)
                            {
                                shapekeySize = 100f;
                                shapekeys.RemoveAt(0);
                            }
                            else
                            {
                                shapekeySize = 0f;
                                keys.SetBlendShapeWeight(prev, shapekeySize);
                                shapekeySize = shapeInit;
                                shapekeys.RemoveAt(0);
                                if (shapekeys.Count > 0)
                                    keys.SetBlendShapeWeight(shapekeys[0], shapekeySize);

                            }
                        }
                    }
                    else
                    {
                        if (shapekeySize <= 100)
                        {
                            keys.SetBlendShapeWeight(shapekeys[0], shapekeySize);
                            shapekeySize += blendSpeed;
                        }
                        else
                        {
                            prev = shapekeys[0];
                            if (shapekeys[1] == -2)
                            {
                                shapekeySize = 100f;

                            }
                            else
                            {
                                if (shapekeys[1] == prev)
                                {
                                    shapekeySize = 100f;
                                    keys.SetBlendShapeWeight(shapekeys[0], shapekeySize);
                                }
                                else
                                {
                                    shapekeySize = 0f;
                                    keys.SetBlendShapeWeight(shapekeys[0], shapekeySize);
                                    shapekeySize = shapeInit;
                                }
                            }
                            shapekeys.RemoveAt(0);
                            if (shapekeys.Count > 0)
                            {
                                if (shapekeys[0] != -2)
                                    keys.SetBlendShapeWeight(shapekeys[0], shapekeySize);
                            }
                        }
                    }
                }
            }
            else
            {
                toAnimate = false;
            }
        }
        /*
        if (toSpeak)
        {
            toSpeak = false;
            UnityEditor.AssetDatabase.ImportAsset("Assets/Sounds/temp.wav");
            audio.Play();            
            //StartCoroutine("speak");
        }
        */
    }

    private List<int> createShapeKeyList(string result)
    {
        List<int> shapekeys = new List<int>();
        List<string> words = new List<string>(result.Split(' '));
        words.RemoveAt(words.Count - 1);

        foreach (string word in words)
        {
            //UnityEngine.Debug.Log(word);
            shapekeys.Add(Convert.ToInt32(word) - 1);
        }
        return shapekeys;
    }

    private void addtoTextArea(int mode, string text)
    {
        if (mode == 0)
        {
            textArea.text += "User: " + text + "\n";
        }
        else
        {
            textArea.text += "AI: " + text + "\n";
        }
    }


    public void createViseme()
    {
        string text = input.text;
        input.text = "";

        try
        {
            if (text == "-1")
            {
                input.enabled = false;
                button.interactable = false;
                UnityEngine.Debug.Log("Exited");
            }
            else
            {
                StartCoroutine(GetViseme(text));
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.HResult & 0x0000FFFF);
        }

    }

    IEnumerator GetViseme(string text)
    {
        button.interactable = false;
        using (UnityWebRequest www = UnityWebRequest.Get(URL + text))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                using (var read = new StreamReader(stream))
                {
                    string result;
                    addtoTextArea(0, text);
                    result = read.ReadLine();
                    addtoTextArea(1, result);
                    result = read.ReadLine();
                    shapekeys.Clear();
                    shapekeys = createShapeKeyList(result);
                    //toSpeak = true;
                    toAnimate = true;
                }
            }
        }
        button.interactable = true;
    }


}
