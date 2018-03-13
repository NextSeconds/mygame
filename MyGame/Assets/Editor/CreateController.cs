using UnityEngine;
using System.Collections;
using UnityEditor.Animations;
using UnityEditor;

public class CreateController : Editor {

    [MenuItem("CreatAnimator/CreateDynamicController")]

    static void Run() {
        //怎么处理带多个动画的对象？？？
        //生成并获取控制器对象
        AnimatorController dynamicController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/AniControllers/DynamicController.controller");
        AnimatorStateMachine rootStateMachine = dynamicController.layers[0].stateMachine;   //声明并初始化根动画状态机
        AnimatorState[] states = new AnimatorState[10];                     //声明动画状态单元集合
        for (int i = 0; i < states.Length; i++) {                           //向状态机中添加动画状态
            states[i] = rootStateMachine.AddState("state" + i);
            states[i].speed = 1.5f;
            //为动画状态指定Behaviour
            //StateMachineBehaviour dynamicBehaviour = AssetDatabase.LoadAssetAtPath("Assets/Scripts/StateBehaviours/DynamicBehaviour.cs", typeof(DynamicBehaviour)) as DynamicBehaviour;
            //states[i].behaviours = new StateMachineBehaviourInfo[10];
            //states[i].behaviours[0].stateMachineBehaviour = dynamicBehaviour;
            //Debug.Log(states[i].behaviours.Length);
        }
        rootStateMachine.defaultState = states[0];
        AnimationClip[] anis = new AnimationClip[10];                       //声明动画片段集合
        for (int i = 0; i < anis.Length; i++) {                             //获取动画片段
            anis[i] = AssetDatabase.LoadAssetAtPath("Assets/Animations/AnisWithNum/Ani" + i + ".FBX", typeof(AnimationClip)) as AnimationClip;
            states[i].motion = anis[i];                                     //设置动画状态中的动画片段
            states[i].iKOnFeet = false;
        }

        for (int i = 0; i < states.Length; i++) {                           //构建动画状态之间的动画过渡
            for (int j = 0; j < states.Length; j++) {
                    dynamicController.AddParameter("state" + i + "TOstate" + j, AnimatorControllerParameterType.Trigger);   //在动画控制器中生成一个触发器参数
                    AnimatorStateTransition trans = states[i].AddTransition(states[j], false);          //为当前两个动画状态之间生成触发器
                    trans.AddCondition(AnimatorConditionMode.If, 0, "state" + i + "TOstate" + j);       //为该触发器指定一个参数
            }
        }
        states[states.Length - 1].AddExitTransition();          //指定输出动画
    }
}
