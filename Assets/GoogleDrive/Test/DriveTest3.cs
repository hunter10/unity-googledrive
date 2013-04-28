﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DriveTest3 : MonoBehaviour
{
	GoogleDrive drive;

	void Start()
	{
		StartCoroutine(InitGoogleDrive());

		#region ESCAPE
		//string a = @"title contains '한글'";
		//byte[] aa = System.Text.Encoding.UTF8.GetBytes(a);
		//string b = "";
		//for (int i = 0; i < aa.Length; i++)
		//{
		//    char c = (char)aa[i];

		//    if ('a' <= c && c <= 'z' ||
		//        'A' <= c && c <= 'Z' ||
		//        '0' <= c && c <= '9')
		//    {
		//        b += c;
		//    }
		//    else if (c == ' ')
		//    {
		//        b += '+';
		//    }
		//    else
		//    {
		//        b += "%" + aa[i].ToString("x2");
		//    }
		//}
		//Debug.Log(b);
		#endregion
	}

	bool initInProgress = false;

	IEnumerator InitGoogleDrive()
	{
		initInProgress = true;

		drive = new GoogleDrive();
		drive.ClientID = "897584417662-rnkgkl5tlpnsau7c4oc0g2jp08cpluom.apps.googleusercontent.com";
		drive.ClientSecret = "tGNLbYnrdRO2hdFmwJAo5Fbt";
		drive.RootDirectoryName = "UnityGoogleDriveTest";

		var authorization = drive.Authorize();
		yield return StartCoroutine(authorization);

		//StartCoroutine(authorization);
		//while (!GoogleDrive.IsDone(authorization))
		//    yield return null;

		if (authorization.Current is Exception)
			Debug.LogWarning(authorization.Current as Exception);
		else
			Debug.Log("User Account: " + drive.UserAccount);

		//// add a folder
		//{
		//    var i = drive.InsertFolder("kekeke", drive.AppData.ID);
		//    yield return StartCoroutine(i);
		//    Debug.Log("" + GoogleDrive.GetResult<GoogleDrive.File>(i));
		//}

#if true
		{
			var listFiles = drive.ListFiles(drive.AppData.ID);
			yield return StartCoroutine(listFiles);
			var files = GoogleDrive.GetResult<List<GoogleDrive.File>>(listFiles);

			if (files != null)
			{
				bool first = false;

				foreach (var file in files)
				{
					Debug.Log(file);

					if (!first)
					{
						first = true;
						//yield return StartCoroutine(drive.DeleteFile(file.ID));

						//file.Title = "world";
						//file.Description = null;
						//yield return StartCoroutine(drive.UpdateFile(file));

						//yield return StartCoroutine(drive.TouchFile(file.ID));
					}

					if (file.Title.EndsWith(".txt"))
					{
						//yield return StartCoroutine(drive.DuplicateFile(file, file.Title + " (2)"));
					}
				}
			}
			else
			{
				Debug.LogError(listFiles.Current);
			}
		}
#endif

#if false
		{
			var data = Encoding.UTF8.GetBytes("now is " + DateTime.Now);

			var upload = drive.UploadFile(new GoogleDrive.File(
				new Dictionary<string, object>
			    {
			        { "title", "my text file.txt" },
			        { "mimeType", "text/plain" },
			        { "description", "hihihi" }
			    }),
				data);

			yield return StartCoroutine(upload);

			var file = GoogleDrive.GetResult<GoogleDrive.File>(upload);

			if (file != null)
			{
				Debug.Log(file);

				var duplicate = drive.DuplicateFile(file, "my test file dup.txt");

				yield return StartCoroutine(duplicate);

				var dupFile = GoogleDrive.GetResult<GoogleDrive.File>(duplicate);

				if (dupFile != null)
				{
					Debug.Log(dupFile);
				}
				else
				{
					Debug.LogError(duplicate.Current as Exception);
				}
			}
			else
			{
				Debug.LogError(upload.Current as Exception);
			}
		}
#endif

		initInProgress = false;
	}

	bool revokeInProgress = false;

	IEnumerator Revoke()
	{
		revokeInProgress = true;

		yield return StartCoroutine(drive.Unauthorize());

		revokeInProgress = false;
	}

	void OnGUI()
	{
		if (initInProgress)
		{
			GUI.enabled = false;
			GUI.Button(new Rect(10, 10, 200, 90), "Init");
			GUI.enabled = true;
		}
		else if (GUI.Button(new Rect(10, 10, 200, 90), "Init"))
		{
			StartCoroutine(InitGoogleDrive());
		}

		if (drive == null || revokeInProgress || !drive.IsAuthorized)
		{
			GUI.enabled = false;
			GUI.Button(new Rect(10, 110, 200, 90), "Revoke");
			GUI.enabled = true;
		}
		else if (GUI.Button(new Rect(10, 110, 200, 90), "Revoke"))
		{
			StartCoroutine(Revoke());
		}
	}
}
