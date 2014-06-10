﻿using NUnit.Framework;
using System;
using Appium.Samples.Helpers;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Threading;
using System.Drawing;

namespace Appium.Samples
{
	[TestFixture ()]
	public class IosSimpleTest
	{
		private AppiumDriver driver;
		private bool onePassed;
		private bool allPassed = true;

		private Random rnd = new Random();

		[TestFixtureSetUp]
		public void BeforeAll(){
			DesiredCapabilities capabilities = Caps.getIos71Caps (Apps.get("iosTestApp")); 
			if (Env.isSauce ()) {
				capabilities.SetCapability("username", Environment.GetEnvironmentVariable("SAUCE_USERNAME")); 
				capabilities.SetCapability("accessKey", Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY"));
				capabilities.SetCapability("name", "ios - simple");
				capabilities.SetCapability("tags", new string[]{"sample"});
			}
			Uri serverUri = Env.isSauce () ? AppiumServers.sauceURI : AppiumServers.localURI;
			driver = new AppiumDriver(serverUri, capabilities);	
		}

		[TestFixtureTearDown]
		public void AfterAll(){
			try
			{
				((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (allPassed ? "passed" : "failed"));
			}
			finally
			{
				driver.Quit();
			}
		}

		[SetUp]
		public void BeforeEach(){
			onePassed = false;
		}

		[TearDown]
		public void AfterEach(){
			allPassed = allPassed && onePassed;
		}

		private int Populate() {
			IList<string> fields = new List<string> ();
			fields.Add ("IntegerA");
			fields.Add ("IntegerB");
			int sum = 0;
			for (int i = 0; i < fields.Count; i++) {
				IWebElement el = driver.FindElementByName (fields[i]);
				int x = rnd.Next (1, 10);
				el.SendKeys("" + x);
				sum += x;
			}
			return sum;
		}

		[Test ()]
		public void ComputeSumTestCase ()
		{
			// fill form with random data
			int sumIn = Populate ();

			// compute and check the sum
			driver.FindElementByAccessibilityId ("ComputeSumButton").Click ();
			Thread.Sleep (1000);
			IWebElement sumEl = driver.FindElementByIosUIAutomation ("elements().withName(\"Answer\");");
			int sumOut = Convert.ToInt32 (sumEl.Text);
			Assert.AreEqual (sumIn, sumOut);
			onePassed = true;
		}
	}
}

