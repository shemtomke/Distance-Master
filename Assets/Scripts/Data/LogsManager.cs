using ClosedXML.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor.MemoryProfiler;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LogsManager : MonoBehaviour
{
    private string logsFolderPath;
    private string excelFilePath;

    private void Start()
    {
        // Define the logs folder path
        logsFolderPath = Path.Combine(Application.persistentDataPath, "Logs");
        // Ensure the Logs folder exists
        if (!Directory.Exists(logsFolderPath))
        {
            Directory.CreateDirectory(logsFolderPath);
        }

        // Define the Excel file path
        excelFilePath = Path.Combine(logsFolderPath, "UserLogs.xlsx");

        // Initialize the Excel file if it doesn't exist
        if (!File.Exists(excelFilePath))
        {
            InitializeExcelFile();
        }
    }
    private void InitializeExcelFile()
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("UserLogs");
            worksheet.Cell(1, 1).Value = "UserName";
            worksheet.Cell(1, 2).Value = "ScreenshotImageName";
            worksheet.Cell(1, 3).Value = "TimeTaken";
            worksheet.Cell(1, 4).Value = "DateTime";

            workbook.SaveAs(excelFilePath);
        }
    }

    public void LogUserAttempt(Log log)
    {
        using (var workbook = new XLWorkbook(excelFilePath))
        {
            var worksheet = workbook.Worksheet("UserLogs");

            int lastRow = worksheet.LastRowUsed().RowNumber();
            worksheet.Cell(lastRow + 1, 1).Value = log.userName;
            worksheet.Cell(lastRow + 1, 2).Value = log.screenshotImageName;
            worksheet.Cell(lastRow + 1, 3).Value = log.timeTaken;
            worksheet.Cell(lastRow + 1, 4).Value = log.dateTime;

            workbook.SaveAs(excelFilePath);
        }
    }
}
