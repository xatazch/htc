using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;
    public TextMeshProUGUI LogText;
    private StringBuilder logs = new StringBuilder();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        AuthenticationManager.Instance.SignIn.AddListener(() => AddLog("Signed in as user " + AuthenticationService.Instance.PlayerId));
    }

    public void AddLog(string log)
    {
        logs.AppendLine(log);
    }

    // Update is called once per frame
    void Update()
    {
        LogText.text = logs.ToString();
    }
}
