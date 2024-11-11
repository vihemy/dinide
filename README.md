# Din Ide (The Idea Machine)
## Description
This repo contains part of the source code of the interactive installation **Din Ide** for the purpose of showcasing the code. (See Assets/Scripts to browse my code)

This installation is created in Unity3D and developed for the exhibition Dødvande (Dead Water), which premiered in 2024 at the Danish aquarium Kattegatcentret.

![Installation picture](https://github.com/vihemy/dinide/blob/main/ReadmeAssets/installation.jpg)

In this installation, the user types in an idea of how to improve the Danish aquatic environment, which is transformed by the idea machine into an image that appears in front of the user.

## Setup

- Unity project on PC
- Four screens (one for text input, one for displaying the visualization, and two dashboards for the transformation effect)
- 1 DMX light fixture
- Integration with OpenAI's DALL-E API and Completion API

## Overview:
![UML sequence diagram](https://github.com/vihemy/dinide/blob/main/ReadmeAssets/sequence_diagram_uml.png)

### When an input is given
1. A profanity filter searches the prompt for curse words in Danish, English, and German (the primary languages of Kattegatcentret's guests). If any profanity is detected, the user is asked to create a new prompt.
2. The program sends the user's idea/prompt to OpenAI via an API and requests the generation of an image. Once the image is created, it is downloaded and imported into the Unity program, and displayed on the screen after a series of fantastical sci-fi dashboard animations.
3. The image is saved along with a metadata file containing information about the image (prompt, user name, age, creation time, etc.) and whether it is relevant to themes such as "ocean," "pollution," "farming," "inventions," etc. (this relevance is determined by a chat model which analyzes whether the prompt's content matches these themes).
4. The 15 most recent images that are considered relevant (variable name is "isRelevant") loop in a slideshow on the screen until they are replaced by newer images. If no relevant images are available, the program displays a set of fallback images.
5. All images (both relevant and irrelevant) and metadata files are saved locally for later use.

### Configuration
In the `StreamingAssets` folder, the following files are available for configuration without needing to rebuild the Unity project:

1. **FallbackEntries**: A folder with images that will automatically be added to the slideshow if there are no user-generated images on the computer.
2. **profanity_da.txt, profanity_en.txt, profanity_de.txt**: Lists of curse words in Danish, English, and German, respectively. Any word added to these lists will be considered profane by the program and prohibited.
3. **config.txt**: A document with necessary data to connect to OpenAI's APIs.
    - **API_KEY**: OpenAI API key.
    - **API_IMAGE_URL**: Base URL for DALL-E (image generation).
    - **DALLE_MODEL**: Name of the image model.
    - **DALLE_N**: Number of images to generate at once. Should always be 1.
    - **DALLE_SIZE**: Image resolution.
    - **API_CHAT_URL**: Base URL for completion. Used to assess whether the prompt "isRelevant."
    - **GPT_MODEL**: Chat model of language model
    - **SYSTEM_CONTEXT**: Prompt sent along with the user prompt to assess whether the idea is "relevant." Topics can be added/removed to adjust the criteria for relevance. The list of topics can be modified, but the last two sentences must not be changed.
