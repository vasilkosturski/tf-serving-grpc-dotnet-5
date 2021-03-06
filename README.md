# tf-serving-grpc-dotnet-5
A .NET 5 Console App consuming a TensorFlow 2 model hosted via TensorFlow Serving gRPC Endpoint in Docker.

Read [the related article](https://vkontech.com/tensorflow-serving-grpc-endpoint-in-docker-with-a-net-5-client/) for a detailed overview.

The repository contains the .NET 5 console app and the artifacts required to build and host the Machine Learning model in Docker with TF Serving.
The ML model is a simple MNIST digits predictor.

  ## Required Software ##
- Docker
- Windows 10
- MSVS 2019 or Rider
  
## Running the Example ##
1.	Go to the `src` directory.
2.	Run the `deploy-model.bat` script. This will run the Tensorflow Serving Docker container and host the MNIST model.
3.	Run the `run-client.bat` script. This will build the .NET client app and run it. As a result, you should see the following output message:
`Predicted value: 8`


