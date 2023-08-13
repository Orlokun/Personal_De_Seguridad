using GameDirection;
using UnityEngine;

public class StartSceneButton : MonoBehaviour
{
    private IGameDirector _mGameDirector;
    // Start is called before the first frame update
    void Start()
    {
        _mGameDirector = GameDirector.Instance;
    }

    public void LoadNewGame()
    {
        _mGameDirector.StartNewGame();
    }
}
