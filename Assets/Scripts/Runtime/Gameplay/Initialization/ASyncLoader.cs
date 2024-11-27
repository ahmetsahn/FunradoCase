using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Gameplay.Initialization
{
    public class ASyncLoader : MonoBehaviour
    {
        private const string SCENE_NAME = "Game";
        
        private void Start()
        {
            LoadLevelAsync().Forget();
        }

        private async UniTask LoadLevelAsync()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(SCENE_NAME);
            await loadOperation.ToUniTask();
        }
    }
}