using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.IO;
public class EventWindows : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    protected SerializedObject _serializedObject;
    private AnimEventDataBase AnimEveDataBase;

    private List<AnimEventDataBase> AllAnimEventDataBase = new List<AnimEventDataBase>();

    // Add menu named "My Window" to the Window menu
    [MenuItem("基礎設定/AnimEventSet")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EventWindows window = (EventWindows)EditorWindow.GetWindow(typeof(EventWindows));
        window.Show();
    }

    private void OnEnable()
    {
        _serializedObject = new SerializedObject(this);
    }

    void OnGUI()
    {
        //更新
        _serializedObject.Update();

        //开始检查是否有修改
        EditorGUI.BeginChangeCheck();


        //if (GUILayout.Button("搜尋全部動畫事件")) { SearchAllEventData(); }
        // myString = EditorGUILayout.TextField("搜尋", myString);

        SirenixEditorGUI.BeginBox();
        DrawAnimEveDataBase();
        SirenixEditorGUI.EndBox();

        if (EditorGUI.EndChangeCheck())
        {
            //将对象状态存储在撤销堆栈上。
            Undo.RegisterCompleteObjectUndo(AnimEveDataBase, AnimEveDataBase.name);
            EditorUtility.SetDirty(AnimEveDataBase);
        }

        _serializedObject.ApplyModifiedProperties();
        Repaint();
    }

    public void sss()
    {
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }

    public void SearchAllEventData()
    {
        string[] paths = Directory.GetFileSystemEntries(Application.dataPath + "/Resources/EventDataBase");
        foreach (string path in paths)
        {
            if (path.ToString().EndsWith(".meta")) continue;
            //搜尋Assets位置 回傳位置在第幾個
            int assetPathIndex = path.IndexOf("Assets");
            //取的第X位置後面的字串
            string localPath = path.Substring(assetPathIndex);
            AnimEventDataBase obj = AssetDatabase.LoadAssetAtPath(localPath, typeof(AnimEventDataBase)) as AnimEventDataBase;
            if (AllAnimEventDataBase.Find(x => x.name.Contains(obj.name)) == null) AllAnimEventDataBase.Add(obj);
        }
    }

    public void DrawAnimEveDataBase()
    {
        this.AnimEveDataBase = (AnimEventDataBase)EditorGUILayout.ObjectField("選擇事件資料", this.AnimEveDataBase, typeof(AnimEventDataBase), true, GUILayout.Width(Screen.width * 0.8f + 5), GUILayout.Height(20));

        if (AnimEveDataBase == null) return;

        GUILayout.BeginHorizontal();
        AnimEveDataBase.AnimatorController = (AnimatorController)EditorGUILayout.ObjectField("動畫控制器", AnimEveDataBase.AnimatorController, typeof(AnimatorController), true, GUILayout.Width(Screen.width * 0.95f + 5), GUILayout.Height(20));
        if (AnimEveDataBase.AnimatorController == null) return;
        var Clips = AnimEveDataBase.AnimatorController.animationClips;
        List<string> ClipsStr = new List<string>();
        foreach (var c in Clips)
        {
            var cstr = c.ToString().Replace("(UnityEngine.AnimationClip)", "");
            ClipsStr.Add(cstr);
        }
        if (OnInspectorGUIButton("+", 25, 25, Color.white, Color.yellow))
        {
            AnimEveDataBase.eventInfo.Add(null);
        }
        GUILayout.EndHorizontal();

        SirenixEditorGUI.BeginBox();
        if (AnimEveDataBase.eventInfo.Count != 0)
        {
            for (int i = 0; i < AnimEveDataBase.eventInfo.Count; i++)
            {

                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                if (AnimEveDataBase.eventInfo[i] == null) continue;
                AnimEveDataBase.eventInfo[i].OnshowEvent = EditorGUILayout.Foldout(AnimEveDataBase.eventInfo[i].OnshowEvent, "", true);
                AnimEveDataBase.eventInfo[i].AnimStateValue = EditorGUILayout.Popup(AnimEveDataBase.eventInfo[i].AnimStateValue, ClipsStr.ToArray(), GUILayout.Width(Screen.width * 0.25f), GUILayout.Height(20));
                AnimationClip Clip = AnimEveDataBase.eventInfo[i].AnimClip = Clips[AnimEveDataBase.eventInfo[i].AnimStateValue];
                //指定生命值的宽高
                if (Clip != null)
                {
                    //绘制時間条
                    EditorGUI.ProgressBar(GUILayoutUtility.GetRect(Screen.width * 0.65f, 25), Clip.length, Clip.length.ToString());
                    if (OnInspectorGUIButton("+", 25, 25, Color.white, Color.yellow))
                    {
                        AnimEveDataBase.eventInfo[i].Event.Add(new EventsInfo.IndividualEvent());
                    }
                }

                if (OnInspectorGUIButton("X", 25, 25, Color.white, Color.red))
                {
                    AnimEveDataBase.eventInfo.RemoveAt(i);
                    return;
                }
                GUILayout.EndHorizontal();
                if (AnimEveDataBase.eventInfo[i].OnshowEvent)
                {
                    if (Clip != null && AnimEveDataBase.eventInfo.Count != 0 && AnimEveDataBase.eventInfo[i].Event.Count != 0)
                    {
                        AnimEveDataBase.eventInfo[i].AnimEvent.Clear();
                        SirenixEditorGUI.BeginBox();
                        for (int j = 0; j < AnimEveDataBase.eventInfo[i].Event.Count; j++)
                        {
                            var AnimEvent = AnimEveDataBase.eventInfo[i].Event[j];

                            GUILayout.BeginHorizontal();
                            AnimEvent.OnshowEvent = EditorGUILayout.Foldout(AnimEvent.OnshowEvent, "事件", true);
                            EditorGUI.ProgressBar(GUILayoutUtility.GetRect(Screen.width * 0.25f, 25), AnimEvent.AnimTimeline, AnimEvent.AnimTimeline + "/" + Clip.length.ToString());
                            if (OnInspectorGUIButton("X", 25, 25, Color.white, Color.red))
                            {
                                AnimEveDataBase.eventInfo[i].Event.RemoveAt(j);
                                return;
                            }
                            GUILayout.EndHorizontal();


                            if (AnimEvent.OnshowEvent)
                            {
                                SirenixEditorGUI.BeginBox();
                                AnimEvent.AnimTimeline = EditorGUILayout.FloatField("時間", AnimEvent.AnimTimeline, GUILayout.Width(Screen.width * 0.25f));
                                AnimEvent.AnimTimeline = Mathf.Clamp(AnimEvent.AnimTimeline, 0, Clip.length);
                                AnimEvent.Objtype = (objtype)EditorGUILayout.EnumPopup("事件類型", AnimEvent.Objtype, GUILayout.Width(Screen.width * 0.5f));

                                switch (AnimEvent.Objtype)
                                {
                                    case objtype.Audio:
                                        break;
                                    case objtype.Effect:
                                        AnimEvent.effectinfo.EffectName = EditorGUILayout.TextField("特效名稱", AnimEvent.effectinfo.EffectName, GUILayout.Width(Screen.width * 0.25f));
                                        AnimEvent.effectinfo.EffectEndTime = EditorGUILayout.FloatField("持續時間", AnimEvent.effectinfo.EffectEndTime, GUILayout.Width(Screen.width * 0.25f));


                                        SetStateEvent(Clip, AnimEveDataBase.eventInfo[i].AnimEvent, AnimEvent.AnimTimeline, AnimEvent.Objtype.ToString(), j);
                                        break;
                                    default:
                                        break;
                                }
                                SirenixEditorGUI.EndBox();
                            }

                        }
                        SirenixEditorGUI.EndBox();
                    }
                    AnimationUtility.SetAnimationEvents(Clip, AnimEveDataBase.eventInfo[i].AnimEvent.ToArray());
                    Undo.RegisterCompleteObjectUndo(Clip, Clip.name);
                }
                GUILayout.EndVertical();
            }
        }
        SirenixEditorGUI.EndBox();
    }


    public void SetStateEvent(AnimationClip clip, List<AnimationEvent> Events, float eventtime, string EventState, int numberEvent)
    {
        AnimationEvent evt = new AnimationEvent();
        evt.intParameter = numberEvent;
        evt.time = eventtime;
        evt.functionName = "PlayEvent";
        evt.stringParameter = EventState;
        Events.Add(evt);
    }
    private bool OnInspectorGUIButton(string label, int width, int height, Color textColor, Color backgroundColor)
    {
        Color saveColor = GUI.backgroundColor;
        GUILayoutOption[] options = { GUILayout.Width(width), GUILayout.Height(height) };
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = textColor;
        GUI.backgroundColor = backgroundColor;

        bool pressed = GUILayout.Button(label, style, options);
        GUI.backgroundColor = saveColor;

        return pressed;
    }

    private void OnDisable()
    {
        AllAnimEventDataBase.Clear();
    }
}
