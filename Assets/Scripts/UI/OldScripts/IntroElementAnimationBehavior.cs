using UnityEngine;

public class IntroElementAnimationBehavior : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject sceneBehaviourGO = GameObject.Find("SceneBehaviour");
        if (sceneBehaviourGO != null)
        {
            MainMenuSceneBehaviour sceneBehaviour = sceneBehaviourGO.GetComponent<MainMenuSceneBehaviour>();
            if (sceneBehaviour != null)
                sceneBehaviour.OnIntroAnimationEnd();
        }
    }
}