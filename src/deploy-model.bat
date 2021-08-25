docker pull tensorflow/serving:2.6.0
docker run -t --rm --name=tf-serving-mnist -p 8500:8500 -v "%cd%/mnist-model:/models/model" tensorflow/serving --model_name=mnist-model