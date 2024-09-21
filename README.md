# Audiobook

Audiobook is a console program that will reformat the contents of a folder into an audiobook format so that I can reformat multiple .MP3 files into a single .M4B Apple audiobook format.

## Setup

I have a folder of .MP3 audio files and a ``cover.jpg`` file that is the cover art file for the audiobook.

The structure of the root folder will be similar to this example.

> The Teaching Company - The Odyssey Of Homer

where the full name will be the ``Title`` and everything before the first ``-`` will be the ``Name`` of the author of the book.

This folder will contain the .MP3 audio files and the cover art.

## Running Audiobook

In the current folder run.

```bash
    audiobook
```

When this console program runs it will move the .MP3 files and cover.jpg into the ``audiobooks`` folder and create a ``makebook.ps1`` Powershell script.

This script is used to run the ``sandreas/m4b-tool`` Docker container.

**Note:** make sure you have Docker running.

## Running the Powershell script

```bash
    .\makebook.ps1
```

This will create an .M4B audiobook file named.

> The Teaching Company - The Odyssey Of Homer.m4b

## Format of the makebook.ps1 script

The Powershell file will be similar to the following file.

```bash
$directory = "${PWD}/audiobooks"
$files = Get-ChildItem -Path $directory -Filter *.mp3 | Sort-Object Name
$dockerCommand = "docker run -it --rm -v ""${PWD}/audiobooks:/mnt"" sandreas/m4b-tool:latest merge"

foreach ($file in $files) {
 $dockerCommand += " ""/mnt/$($file.Name)"""

# Write-Host " ""/mnt/$($file.Name)"""
}

$dockerCommand += " --output-file ""/mnt/The Teaching Company - The Odyssey Of Homer.m4b"" --series """" --name=""The Teaching Company - The Odyssey Of Homer"" --series-part=1 --artist ""The Teaching Company"" --albumartist=""The Teaching Company"" --use-filenames-as-chapters --cover ""/mnt/cover.jpg"" --jobs=8 --audio-channels=2 --audio-samplerate=44100"

#Write-Host $dockerCommand

# Execute the command
Invoke-Expression $dockerCommand
```
