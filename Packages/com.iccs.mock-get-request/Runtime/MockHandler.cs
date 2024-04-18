using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MockGetRequest
{
    public class MockHandler : MonoBehaviour
    {
        [SerializeField]
        private string _baseURI;
        
        [SerializeField]
        private string _getEndpoint;

        private List<Handle> _activeHandles = new();

        internal void Complete(Handle handle)
        {
            _activeHandles.Remove(handle);
        }

        public Handle Create(int objectID)
        {
            var newHandle = new Handle($"{_baseURI}{_getEndpoint}{objectID}", this);
            _activeHandles.Add(newHandle);

            return newHandle;
        }

        public class Handle
        {
            public Handle(string uri, MockHandler owner)
            {
                URI = uri;
                Owner = owner;
                _onSuccess = DefaultSuccess;
                _onFail = DefaultFail;
            }

            public string URI { get; private set; }
            public MockHandler Owner { get; private set; }

            private Action<DynamicObjectData> _onSuccess;
            private Action<string> _onFail;

            public Handle OnSuccess(Action<DynamicObjectData> response)
            {
                _onSuccess = response;
                return this;
            }

            public Handle OnFail(Action<string> response)
            {
                _onFail = response;
                return this;
            }

            public Handle GET()
            {
                Owner.StartCoroutine(GetRequest());
                return this;
            }

            IEnumerator GetRequest()
            {
                using UnityWebRequest webRequest = UnityWebRequest.Get(URI);
                webRequest.certificateHandler = new ForceAcceptAll();

                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        _onFail.Invoke($"Error: {webRequest.error}");
                        break;

                    case UnityWebRequest.Result.ProtocolError:
                        _onFail.Invoke("HTTP Error: " + webRequest.error);
                        break;

                    case UnityWebRequest.Result.Success:
                        DynamicObjectData content = new();
                        bool succeeded = false;
                        try
                        {
                            JsonUtility.FromJsonOverwrite(webRequest.downloadHandler.text, content);
                            succeeded = true;
                        }
                        catch (System.Exception)
                        {
                            _onFail.Invoke($"Deserialization Error from response: {webRequest.downloadHandler.text}");
                        }
                        if(succeeded) _onSuccess.Invoke(content);
                        break;
                }

                yield return null;

                Owner.Complete(this);

                Owner = null;
                _onSuccess = null;
                _onFail = null;
            }

            private void DefaultSuccess(DynamicObjectData data)
            {
                Debug.Log($"Returned object name: {data.model_name}");
            }

            private void DefaultFail(string error)
            {
                Debug.LogError(error);
            }

            internal class ForceAcceptAll : CertificateHandler
            {
                protected override bool ValidateCertificate(byte[] certificateData)
                {
                    return true;
                }
            }
        }
    }
}
