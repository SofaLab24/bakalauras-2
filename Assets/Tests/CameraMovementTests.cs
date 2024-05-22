using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem.LowLevel;
using UnityEditor.SceneManagement;


public class CameraMovementTests : InputTestFixture
{
    [UnityTest]
    public IEnumerator CameraMovementTestsWithEnumeratorPasses()
    {
        Mouse mouse = InputSystem.AddDevice<Mouse>();
        EditorSceneManager.OpenScene("Assets/Scenes/GameView.unity");
        var screenTop = new Vector2(Screen.width / 2, Screen.height * 0.98f);
        InputSystem.QueueStateEvent(mouse, new MouseState { position = screenTop });
        InputSystem.Update();
        yield return null; // Wait for a frame to process the input

        for (int i = 0; i < 60; i++)
        {
            yield return null;
        }
        Assert.AreEqual(screenTop, mouse.position.ReadValue());


        // Move the mouse back to the middle of the screen
        var screenMiddle = new Vector2(Screen.width / 2, Screen.height / 2);
        InputSystem.QueueStateEvent(mouse, new MouseState { position = screenMiddle });
        InputSystem.Update();
        yield return null; // Wait for a frame to process the input

        // Assertions to verify the mouse position
        for (int i = 0; i < 60; i++)
        {
            yield return null;
        }
        Assert.AreEqual(screenMiddle, mouse.position.ReadValue());
    }
}
