using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Status_machine;
[System.Serializable] public class Status_Anim
{
    [SerializeField] public Status statu;
    [SerializeField] public AnimationClip clip;
}
public class Animi_Con : MonoBehaviour
{
    Animator anim;//¶¯»­»ú
    [SerializeField] Status_Anim[] tmp;//¶¯»­»º´æ
    Dictionary<Status, string> dict;//¶¯»­×´Ì¬Á´½Ó
    string nam;


    void Awake()
    {
        dict = new Dictionary<Status, string>();
        for(var i = 0; i < tmp.Length; i++)
        {
            dict.Add(tmp[i].statu, tmp[i].clip.name);
        }

    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    ///////////////////////////////////////////////////////
    public void playAnime(Status statu)
    {
        nam = dict[statu];
        anim.Play(nam);
    }
}
