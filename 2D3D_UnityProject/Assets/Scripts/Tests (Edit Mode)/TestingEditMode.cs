using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestingEditMode
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestingEditModeSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestingEditModeWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [Test]
        public void NewSimpleTest()
        {
            GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Assert.IsNotNull(ball);
        }

        // [UnityTest]
        // public IEnumerator NewEnumeratorTest()
        // {
        //     // Increase the timeScale so the test executes quickly
        //     Time.timeScale = 20.0f;
 
        //     GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
 
        //     float time = 0;
        //     while (time < 5)
        //     {
        //         ball.transform.Translate(Vector3.forward * Time.deltaTime);
        //         time += Time.fixedDeltaTime;
	    //         yield return new WaitForFixedUpdate();
        //     }
 
        //     // Reset timeScale
        //     Time.timeScale = 1.0f;

        //     Assert.GreaterOrEqual(ball.transform.position.x, 0);
        // }
    }
}
