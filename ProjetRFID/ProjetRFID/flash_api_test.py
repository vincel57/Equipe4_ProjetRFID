from flask import Flask, request, jsonify
from Analknn import result
from Analknn import knn_classifier
from Analknn import svm
from knnsvm import random_forest_retries
app = Flask(__name__)

@app.route('/result', methods=['POST'])
def result_route():
    # Get the input parameters from the request

    
    # Call the result() function to make a prediction
    analytique = result()
    #prediction = 0.75
    # Return the prediction as JSON
    return jsonify({'Analytique': analytique})




    
    
 @app.route('/knn', methods=['POST'])
def knn_route():
    # Get the input parameters from the request
    input_params = request.get_json()
    
    # Call the predict() function to make a predsiction
    knn = knn_classifier(input_params)
    #prediction = 0.75
    # Return the prediction as JSON
    return jsonify({'Knn': knn})


  @app.route('/svm', methods=['POST'])
def svm_route():
    # Get the input parameters from the request
    input_params = request.get_json()
    
    # Call the predict() function to make a predsiction
    svm = svm(input_params)
    #prediction = 0.75
    # Return the prediction as JSON
    return jsonify({'svm': svm})

   
    
   @app.route('/random', methods=['POST'])
def random_route():
    # Get the input parameters from the request
    input_params = request.get_json()
    
    # Call the predict() function to make a predsiction
    random = random_forest_retries(input_params)
    #prediction = 0.75
    # Return the prediction as JSON
    return jsonify({'random': random})
app.run(host='0.0.0.0', port=5000)
