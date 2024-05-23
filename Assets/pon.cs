using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class pon : MonoBehaviour
{
    public Text header;
    public Text fio;
    public Text city;
    public InputField inputemadre  ;
    public RawImage myTexture;
    public Text name;

    private void Start()
    {
        StartCoroutine(GetText("http://10.0.118.3:5000/", "top"));
    }

    public void GetCity()
    {
        StartCoroutine(GetText("http://10.0.118.3:5000/city", "city"));
    }

    public void GetName()
    {
        StartCoroutine(GetText("http://10.0.118.3:5000/name?name=" + inputemadre.text , "name"));
    }

    public void GetAnimals()
    {
        StartCoroutine(GetText("http://10.0.118.3:5000/animals", "animals"));
    }

    IEnumerator GetText(string link, string choose)
    {
        UnityWebRequest www = UnityWebRequest.Get(link);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;

            if (choose == "top")
            {
                header.text = response;
                Debug.Log( response);
            }
            else if (choose == "city")
            {
                city.text = "no way it:"+ response;
                Debug.Log( response);
            }
            else if (choose == "name")
            {
                fio.text = response;
                Debug.Log( response);
            }
            else if (choose == "animals")
            {
                Match match = Regex.Match(response, @"""url""\s*:\s*""(.*?)""");
                StartCoroutine(GetTexture(match.Groups[1].Value));
                Debug.Log( match.Groups[1].Value);
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    IEnumerator GetTexture(string link)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(link);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            myTexture.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Debug.Log("Texture downloaded successfully.");
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}