using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理的命名空间

public class Teleport : MonoBehaviour
{
    public void TeleportToLevel(string levelName)
    {
        // 调用 SceneManager.LoadSceneAsync 是异步加载，可以避免画面冻结
        // 如果不需要立即返回，可以省略 Async 使用 SceneManager.LoadScene
        SceneManager.LoadScene(levelName); // 传入关卡场景的名称
    }
}
