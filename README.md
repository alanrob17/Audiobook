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
