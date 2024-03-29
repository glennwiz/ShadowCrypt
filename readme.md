# ShadowCrypt - Steganography Console Tool

## Overview

ShadowCrypt is a simple yet powerful steganography console tool that allows you to hide secret messages within digital images. This tool leverages the concept of altering the least significant bits (LSB) of the RGB components of image pixels to encode and decode hidden information.

## Features

- **Encoding:** Easily encode your secret messages into images, making them appear as regular images to the naked eye.
- **Decoding:** Retrieve hidden messages from encoded images using ShadowCrypt's decoding functionality.
- **Command-Line Interface:** User-friendly command-line interface with options to encode and decode messages.
- **Image Format Support:** ShadowCrypt supports commonly used image formats, including PNG.

## Usage

### Encoding a Message

To encode a message into an image, use the following command:

```shell
shadowcrypt -encode -i input_image.png -o output_image.png -m "Your secret message"
shadowcrypt -decode -i encoded_image.png
```

## Installation
Clone this repository to your local machine.

Build the project using Visual Studio or the .NET CLI.

Run the ShadowCrypt tool from the command line as described in the "Usage" section.

## License
This project is licensed under the MIT License.

## Acknowledgments
ShadowCrypt was created by Glennwiz as a fun project to explore steganography techniques. Feel free to contribute, report issues, or provide feedback to make it even better!
