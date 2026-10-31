using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SyraxServerConnector : MonoBehaviour
{
    [Header("Server Settings")]

    // إذا Unity على نفس الكمبيوتر استخدمي localhost.
    // إذا على Quest أو جهاز آخر اكتبي IP الكمبيوتر، مثل:
    // http://192.168.1.50:3000
    [SerializeField]
    private string serverUrl = "http://localhost:3000";

    [Header("Optional Scene Names")]
    [SerializeField]
    private string phoneCallScene = "PhoneCall";

    [SerializeField]
    private string messageScene = "Message";

    [SerializeField]
    private string websiteScene = "Website";

    public SyraxSession CurrentSession { get; private set; }

    public void GetLatestSession()
    {
        StartCoroutine(GetLatestSessionCoroutine());
    }

    private IEnumerator GetLatestSessionCoroutine()
    {
        string url = $"{serverUrl}/api/unity/session";

        Debug.Log($"SYRAX: Requesting session from {url}");

        using UnityWebRequest request = UnityWebRequest.Get(url);

        request.timeout = 10;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                $"SYRAX connection failed.\n" +
                $"Status Code: {request.responseCode}\n" +
                $"Error: {request.error}\n" +
                $"Response: {request.downloadHandler?.text}"
            );

            yield break;
        }

        string json = request.downloadHandler.text;

        Debug.Log($"SYRAX session received:\n{json}");

        try
        {
            CurrentSession = JsonUtility.FromJson<SyraxSession>(json);
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"SYRAX JSON parsing failed: {exception.Message}"
            );

            yield break;
        }

        if (CurrentSession == null)
        {
            Debug.LogError("SYRAX returned an empty session.");
            yield break;
        }

        Debug.Log(
            $"User: {CurrentSession.user_profile.display_name}\n" +
            $"Primary risk: {CurrentSession.risk_profile.primary_risk}\n" +
            $"Current stage: {CurrentSession.current_stage.type}\n" +
            $"Session ID: {CurrentSession.session_id}"
        );

        OpenCurrentStage();
    }

    private void OpenCurrentStage()
    {
        if (CurrentSession?.current_stage == null)
        {
            Debug.LogError("SYRAX current stage is missing.");
            return;
        }

        string stageType = CurrentSession.current_stage.type;

        switch (stageType)
        {
            case "PhoneCall":
                LoadSceneIfValid(phoneCallScene);
                break;

            case "Message":
                LoadSceneIfValid(messageScene);
                break;

            case "Website":
                LoadSceneIfValid(websiteScene);
                break;

            default:
                Debug.LogError(
                    $"Unknown SYRAX stage type: {stageType}"
                );
                break;
        }
    }

    private void LoadSceneIfValid(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Scene name is empty.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError(
                $"Scene '{sceneName}' was not found. " +
                "Add it to File > Build Profiles > Scene List."
            );

            return;
        }

        Debug.Log($"SYRAX: Loading scene {sceneName}");

        SceneManager.LoadScene(sceneName);
    }
}

#region JSON Models

[Serializable]
public class SyraxSession
{
    public string session_id;
    public string engine_mode;
    public string stage_engine_mode;
    public string status;

    public SyraxUserProfile user_profile;
    public SyraxRiskProfile risk_profile;
    public SyraxStage current_stage;

    public int current_stage_index;
    public string[] stage_order;
}

[Serializable]
public class SyraxUserProfile
{
    public string display_name;
    public string age_group;
    public string financial_experience;
    public string language;
}

[Serializable]
public class SyraxRiskProfile
{
    public string primary_risk;
    public int risk_score;
    public string[] weaknesses;
    public string reasoning;
}

[Serializable]
public class SyraxStage
{
    public string type;
    public string scenario_id;
    public string title;
    public string actor_name;
    public string opening_text;
    public string body_text;
    public string call_to_action;
    public string fake_url;
    public string goal;
    public string visual_hint;
}

#endregion