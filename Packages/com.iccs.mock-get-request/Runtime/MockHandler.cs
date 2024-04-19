using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MockGetRequest
{
    /// <summary>
    /// Provides Mock Http call functionality.
    /// Create a GameObject with this component to use it.
    /// Call the Create() method to get a Handler.
    /// And then the GET() method on the handler to make the http call.
    /// Use OnSuccess() and OnFail() methods on the handler to receive callbacks.
    /// </summary>
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

        /// <summary>
        /// This is a helper class for the MockHandler to manage an individual web request
        /// </summary>
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
                        DynamicObjectData content;
                        bool succeeded = false;
                        try
                        {
                            content = JsonUtility.FromJson<DynamicObjectData>(webRequest.downloadHandler.text);
                            succeeded = true;
                        }
                        catch (System.Exception)
                        {
                            _onFail.Invoke($"Deserialization Error from response: {webRequest.downloadHandler.text}");
                            content = new();
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

            /// <summary>
            /// Used to accept all Web Certificates for test purposes.
            /// </summary>
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
