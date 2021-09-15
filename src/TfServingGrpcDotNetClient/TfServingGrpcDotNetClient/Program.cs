using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Tensorflow;
using Tensorflow.Serving;

namespace TfServingGrpcDotNetClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string imagePath = @"mnist_test_eight.png";
            var input = PreprocessTestImage(imagePath);

            var request = new PredictRequest
            {
                ModelSpec = new ModelSpec { Name = "mnist-model" }
            };
            
            request.Inputs.Add("flatten_input", input);
            
            var channel = new Channel("localhost:8500", ChannelCredentials.Insecure);
            var client = new PredictionService.PredictionServiceClient(channel);
            
            var predictResponse = await client.PredictAsync(request);
            
            var maxValue = predictResponse.Outputs["dense_1"].FloatVal.Max();
            var predictedValue = predictResponse.Outputs["dense_1"].FloatVal.IndexOf(maxValue);

            Console.WriteLine($"Predicted value: {predictedValue}");
        }
        
        private static TensorProto PreprocessTestImage(string path)
        {
            var img = new Bitmap(path);
            var imgTransformed = new float[img.Width][];
            
            for (int i = 0; i < img.Width; i++)
            {
                imgTransformed[i] = new float[img.Height];
                for (int j = 0; j < img.Height; j++)
                {
                    var pixel = img.GetPixel(i, j);
                    
                    var gray = RgbToGray(pixel);
                    
                    // Normalize the Gray value to 0-1 range
                    var normalized = gray / 255;
                    
                    imgTransformed[i][j] = normalized;
                }
            }

            return CreateTensorFromImage(imgTransformed);
        }

        private static float RgbToGray(Color pixel) => 0.299f * pixel.R + 0.587f * pixel.G + 0.114f * pixel.B;

        private static TensorProto CreateTensorFromImage(float[][] imageData)
        {
            var imageFeatureShape = new TensorShapeProto();

            imageFeatureShape.Dim.Add(new TensorShapeProto.Types.Dim() { Size = 1 });
            imageFeatureShape.Dim.Add(new TensorShapeProto.Types.Dim() { Size = imageData.Length * imageData.Length });

            var imageTensorBuilder = new TensorProto
            {
                Dtype = DataType.DtFloat,
                TensorShape = imageFeatureShape
            };

            for (int i = 0; i < imageData.Length; ++i)
            {
                for (int j = 0; j < imageData.Length; ++j)
                {
                    imageTensorBuilder.FloatVal.Add(imageData[i][j]);
                }
            }

            return imageTensorBuilder;
        }
    }
}
