from flask import Flask, request, jsonify
from Analknn import result
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
    knn = knn(input_params)
    #prediction = 0.75
    # Return the prediction as JSON
    return jsonify({'Knn': knn})


app.run(host='0.0.0.0', port=5000)
