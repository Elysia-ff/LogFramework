using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace LogFramework
{
    public class LogSystem : MonoBehaviour
    {
        #region Singleton
        private static LogSystem m_Instance = null;
        public static LogSystem Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType<LogSystem>();
                    if (m_Instance == null)
                    {
                        m_Instance = new GameObject(typeof(LogSystem).ToString(), typeof(LogSystem)).GetComponent<LogSystem>();
                    }
                }

                return m_Instance;
            }
        }

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
        }

        private void OnEnable()
        {
            if (m_Instance == null)
                m_Instance = this;
        }

        private void OnDestroy()
        {
            m_Instance = null;
        }
        #endregion

        // TODO (LogFramework) ::
        // Define here your log server address.
        private static readonly string URL = "<your log server address>";

        public void Send(LogBase log)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("Log sent : {0}\n{1}", log.LogName, log.ToString()));
#endif

            StartCoroutine(SendRoutine(log));
        }

        private IEnumerator SendRoutine(LogBase log)
        {
            using (var request = new UnityWebRequest(URL, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(log.ToJSON());
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

#if UNITY_EDITOR
                // It's just a log so Ignore the error on build. 
                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError(request.error);
                }
#endif
            }
        }
    }
}
