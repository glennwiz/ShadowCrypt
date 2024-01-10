using CommandLine;
using System.Drawing;
using System.Drawing.Imaging;

public class Options
{
    [Option('e', "encode", Required = false, HelpText = "Encode a message into an image.")]
    public string? Encode { get; set; }

    [Option('d', "decode", Required = false, HelpText = "Decode a message from an image.")]
    public string? Decode { get; set; }

    [Option('i', "input", Required = true, HelpText = "Input image file path.")]
    public string? InputImage { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output image file path.")]
    public string? OutputImage { get; set; }

    [Option('m', "message", Required = false, HelpText = "Message to encode.")]
    public string? Message { get; set; }
}

public class SteganographyTool
{
    public static void Encode(string imagePath, string message, string outputPath)
    {
        try
        {
            // Load the image
            var originalImage = new Bitmap(imagePath);
            var messageIndex = 0;

            // Check if the message length is within bounds
            if (message.Length * 8 > originalImage.Width * originalImage.Height)
            {
                Console.WriteLine("Error: Message is too large to encode in the image.");
                return;
            }

            // Iterate through the image pixels
            for (var y = 0; y < originalImage.Height; y++)
            {
                for (var x = 0; x < originalImage.Width; x++)
                {
                    var pixel = originalImage.GetPixel(x, y);

                    // Retrieve the RGB components
                    var r = pixel.R;
                    var g = pixel.G;
                    var b = pixel.B;

                    // Get the current message byte to encode
                    byte messageByte = 0;
                    if (messageIndex < message.Length)
                    {
                        messageByte = (byte)message[messageIndex];
                    }

                    // Modify the least significant bits of RGB to encode the message
                    r = (byte)((r & 0xFE) | ((messageByte >> 7) & 1));
                    g = (byte)((g & 0xFE) | ((messageByte >> 6) & 1));
                    b = (byte)((b & 0xFE) | ((messageByte >> 5) & 1));

                    // Update the pixel with modified RGB values
                    originalImage?.SetPixel(x, y, Color.FromArgb(r, g, b));

                    // Move to the next message byte
                    messageIndex++;

                    // Check if we have encoded the entire message
                    if (messageIndex >= message.Length)
                    {
                        break;
                    }
                }
                if (messageIndex >= message.Length)
                {
                    break;
                }
            }

            // Save the modified image with the hidden message
            originalImage?.Save(outputPath, ImageFormat.Png);
            Console.WriteLine("Message encoded successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public static string Decode(string imagePath)
    {
        try
        {
            // Load the image
            var encodedImage = new Bitmap(imagePath);
            var decodedMessage = "";
            var messageLength = 0;

            // Iterate through the image pixels
            for (var y = 0; y < encodedImage.Height; y++)
            {
                for (var x = 0; x < encodedImage.Width; x++)
                {
                    var pixel = encodedImage.GetPixel(x, y);

                    // Retrieve the least significant bit of each RGB component
                    var lsbR = (byte)(pixel.R & 1);
                    var lsbG = (byte)(pixel.G & 1);
                    var lsbB = (byte)(pixel.B & 1);

                    // Combine the LSBs to reconstruct the hidden message
                    var messageByte = (byte)((lsbR << 7) | (lsbG << 6) | (lsbB << 5));

                    // Add the retrieved byte to the decoded message
                    decodedMessage += (char)messageByte;

                    // Check for the end of the message
                    if (messageByte == 0)
                    {
                        messageLength = decodedMessage.Length;
                        break;
                    }
                }
                if (messageLength > 0)
                {
                    break;
                }
            }

            // Trim any extra characters beyond the actual message
            if (messageLength > 0)
            {
                decodedMessage = decodedMessage.Substring(0, messageLength - 1);
            }

            return decodedMessage;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "Error decoding the message.";
        }
    }

    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options =>
            {
                if (!string.IsNullOrWhiteSpace(options.Encode))
                {
                    Encode(options.InputImage, options.Message, options.OutputImage);
                    Console.WriteLine("Message encoded successfully!");
                }
                else if (!string.IsNullOrWhiteSpace(options.Decode))
                {
                    var decodedMessage = Decode(options.InputImage);
                    Console.WriteLine("Decoded message: " + decodedMessage);
                }
                else
                {
                    Console.WriteLine("Please specify either -encode or -decode flag.");
                }
            });
    }
}