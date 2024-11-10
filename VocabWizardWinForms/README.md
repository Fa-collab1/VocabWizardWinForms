# VocabWizardWinForms - Application Readme and User Manual

## Overview

VocabWizardWinForms is a Windows Forms application designed to facilitate vocabulary training by allowing users to create, filter, and practice vocabulary exercises stored in Excel files. The application offers features such as dynamic filtering, self-assessment, and performance tracking. This readme also serves as a comprehensive user manual.

## Table of Contents

1. [Installation](#installation)
2. [Getting Started](#getting-started)
3. [Excel File Format](#excel-file-format)
4. [Application Features](#application-features)
5. [Using the Application](#using-the-application)
6. [Practice Modes](#practice-modes)
7. [Customizing and Managing Filters](#customizing-and-managing-filters)
8. [Clearing Scores](#clearing-scores)
9. [File Monitoring](#file-monitoring)
10. [Troubleshooting](#troubleshooting)
11. [Contact and Support](#contact-and-support)

## Installation

1. Clone or download the latest version of VocabWizardWinForms from the repository at [https://github.com/Fa-collab1/VocabWizardWinForms](https://github.com/Fa-collab1/VocabWizardWinForms).
2. Open the solution in Visual Studio.
3. Build the solution to generate the executable file.
4. Ensure that .NET Framework (version 4.7.2 or higher) is installed on your machine.
5. Run `VocabWizardWinForms.exe` from the `bin` directory to launch the application.

## Getting Started

When you launch VocabWizardWinForms:
- Ensure you have your vocabulary exercises stored in the `#Excel` directory located within the application folder.
- The application will automatically load these files and display available options for practice and filtering.
- The application is pre-loaded with five example data practice set Excel files.

## Excel File Format

To ensure proper functionality, your Excel files must follow a specific format:

- **Sheet Layout**: The first sheet of the Excel file should contain the data.
- **Row One**: Reserved for column titles.
- **Column Structure**:
  - **Column 1 (A)**: Study Language – The language of the original word.
  - **Column 2 (B)**: Word Class – The grammatical category (e.g., noun, verb).
  - **Column 3 (C)**: Chapter – Chapter number or topic indicator.
  - **Column 4 (D)**: Translation – Translation of the original word.
  - **Column 5 (E)**: Original – The word/phrase in the original language.
  - **Column 6 (F)**: Update Date – The date the exercise was last updated (no need to pre-fill).
  - **Columns 7-11 (G-K)**: Points – Fields for tracking the latest points (e.g., LatestPoint, SecondLatestPoint, etc.) (no need to pre-fill).
  - **Column 12 (L)**: Dirty Status – A marker to indicate exercises that have been modified (use '1' or 'TRUE' for rows needing correction; this can also be toggled within the application).

Ensure that all columns are filled with relevant data and do not contain formatting that may interfere with reading (e.g., merged cells or hidden rows).

## Application Features

- **Excel Integration**: Load and manage vocabulary exercises from Excel files.
- **Dynamic Filtering**: Filter exercises based on criteria such as language, word class, chapter, and more.
- **Self-assessment**: Practice exercises with built-in scoring mechanisms.
- **Color-coded Visual Cues**: Distinguish between selected and non-selectable filter options.
- **File Monitoring**: Auto-detect changes in the `Excel` directory for seamless updates.
- **Two Practice Modes**: Choose between one-card singular practice with self-assessment or a five-option version with automatic grading.
- **Stored Statistics**: Your progress is saved, and results are filterable.

## Using the Application

### Main Interface

The main window consists of several group boxes for filters and options:

- **Language Filter**: Select specific languages.
- **Word Class Filter**: Choose from various word classes.
- **Chapter Filter**: Filter by chapter.
- **File Name Filter**: Select specific files.
- **Average Points Filter**: Narrow down based on average points.
- **Dirty Status Filter**: Filter by exercises marked as “dirty” or “clean.”

### Filter Interaction

- Select and deselect filters to adjust the exercise list.
- Non-selectable options are visually marked but remain actionable.

## Practice Modes

VocabWizardWinForms offers two main practice modes:

1. **One Card Mode**: Practice one data row at a time with self-assessment scoring.
2. **Five Card Mode**: Practice with a set of five answer alternatives to choose from with automatic grading.
- Mark exercises that need to be corrected with the dirty mark for easy access in the excel file later.
- Points that are given and stored are [0 ; 0.125 ; 0.25 ; 0.5 ; 1]

### Starting a Practice Session

- Click the "Practice" button to start a session.
- Choose between practicing into or from a language.
- Review and self-assess your answers using the provided scoring buttons.

## Customizing and Managing Filters

### Applying Filters

1. Select your desired filters from the group boxes.
2. The filtered list updates automatically, showing the number of matching exercises and a percentage score.

### Resetting Filters

- Click the “Reset Filters” button to clear all selected filters and reset the options.

## Clearing Scores

To reset scores for specific exercises:

1. Apply the necessary filters to select exercises.
2. Click the “Clear Scores” button. The scores in the filtered exercises will be reset, and the affected files will be saved.

## File Monitoring

The application includes a built-in file watcher:

- It monitors changes in the `Excel` directory.
- Updates are detected when a file is added, modified, or removed.
- The application reloads the updated data automatically.

## Troubleshooting

### Common Issues

- **File Access Errors**: Ensure Excel files are not open in another application when loading or saving.

### Tips

- If a checkbox is marked red, it indicates a selected option that is no longer available based on the current filters.
- Non-selectable options will be disabled but still visible.
