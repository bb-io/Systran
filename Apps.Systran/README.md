# Blackbird.io Systran

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Systran Translate API is built around a RESTful API and can be used in all types of applications.
This API Reference is intended for developers who want to write applications that can interact with the SYSTRAN Translate API. 
You can integrate our translation technology directly in your internal or external applications to make them multilingual or translate texts and files by using the SYSTRAN Translate API.

## Before setting

Before you can connect you need to make sure that:
- You have a Systran account and [Set up your account](https://spns-dev-web.systran.us/en/authentication)
- You have created the API key in  `my account -> setting -> API keys`

## Connecting
1.Navigate to apps and search for Systran
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My Systran connection'.
4. Fill in your API key obtained earlier.
5. Fill in your Instance URL.
6. Click _Connect_.


## Actions

### Corpus

- **Export corpus** Export a corpus file in `.tmx` format by its ID
- **Import corpus from TMX file** Add a new corpus from an `.tmx` file.

### Dictionary

- **Create dictionary** Generate a dictionary and populate it from a `.tbx` file
- **Export dictionary** Export dictionary as `.tbx` file
- **Update dictionary from TBX file** Import entries from a `.tbx` file to update an existing dictionary.


### Translation

- **Translate** Translate a file from source language to target language. There is 2 strategies 'Systran' and 'Blackbird'. 'Systran' strategy uses native functionality. 'Blackbird' strategy uses Blackbird's translation the file and it is the default, to change it, please use property 'File Translation Strategy'. 
- **Translate file (Async)** Translate a file from source language to target language
- **Download translated file** Download a translated file by request ID
- **Translate text** Translate text

## Events

- **On translation finished** Triggered when the translation status is finished

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
