using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    #region variables

    [Header("Component")]
    private static GameObject _alert;
    private static Text _message;

    #endregion variables

    #region methods

    // creates the alert instance
    private static void Create()
    {
        // create instance of alert prefab as gameobject
        _alert = Instantiate(Resources.Load<GameObject>("Alert"));

        // ensure everything sticks around like a bad smell
        DontDestroyOnLoad(_alert);

        // find all the required components
        _message = _alert.GetComponentInChildren<Text>();

        // deactivate alert
        _alert.SetActive(false);
    }

    // used to show the alert from external sources
    // time is optional
    public static void Show(string message, float time = 2.5f)
    {
        Create();

        // set the message text
        _message.text = message;

        // after everything has been set, show the alert
        _alert.SetActive(true);

        // start coroutine to hide alert if time is greater than zero
        if (time > 0)
            _alert.GetComponent<Alert>().StartCoroutine(Hide(time));
    }

    // used to hide the alert from external sources if time is set to zero
    public static void Hide()
    {
        // hide the modal
        _alert.SetActive(false);
    }

    // used to hide the alert from internal source is time is set to greater than zero
    private static IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(time);
        _alert.SetActive(false);
    }

    #endregion methods
}