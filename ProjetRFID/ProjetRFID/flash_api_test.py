from flask import Flask, request, jsonify
from ML_RFID_maj import result, knn_classifier, SVM, pre_traitement, unzip_folder, est_fichier_zip, RandomForestML, dataset
import os
import zipfile

app = Flask(__name__)

def est_fichier_zip(chemin_fichier):
    """ Verifie si le fichier specifie est un fichier zip en examinant son extension. """
    return os.path.splitext(chemin_fichier)[1].lower() == '.zip'
 
def unzip_folder(zip_file, output_dir):
    """
    Decompresse un fichier zip et place les dossiers extraits dans un repertoire specifie.
 
    Args:
        zip_file (str): Chemin vers le fichier zip a decompresser.
        output_dir (str): Chemin vers le repertoire de sortie ou placer les dossiers extraits.
    """
    # Verifier si le repertoire de sortie existe, sinon le creer
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)
 
    # Ouvrir le fichier zip en mode lecture
    with zipfile.ZipFile(zip_file, 'r') as zip_ref:
        # Extraire tous les fichiers dans le repertoire de sortie
        zip_ref.extractall(output_dir)
    #print("La decompression est terminee.")



file_path='C:/Users/33760/Downloads/Equipe4_ProjetRFID/ProjetRFID/ProjetRFID/data_anonymous'
directory= 'C:/Users/33760/Downloads/Equipe4_ProjetRFID/ProjetRFID/ProjetRFID/Folder'

if est_fichier_zip(file_path):
    d = directory
    unzip_folder(file_path, directory)
else:
    d=file_path
pathfile = d 



# Chemin du dossier
if os.path.exists(file_path):
    
    # Le dossier existe, donc tu peux executer la fonction de pretraitement
    pretrait = pre_traitement(file_path)
    data = dataset(pretrait[0], pretrait[1], pretrait[2], pretrait[3])
 
    print('fichier exist : ')


@app.route('/result', methods=['POST'])
def result_route():
    try:
        # Call the result() function to make a prediction
        analytique = result()
        # Return the prediction as JSON
        return jsonify({'Analytique': analytique}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500


@app.route('/knn', methods=['POST'])
def knn_route():
    try:
        # Get the input parameters from the request
        input_params = request.get_json()
        if not input_params:
            return jsonify({'error': 'Invalid input parameters'}), 400
        print( input_params['metric_params'])
        # Call the knn_classifier() function to make a prediction
        accuracy_knn = knn_classifier(data,
                              input_params['n_neighbors'],
                              input_params['weight'],
                              input_params['metric'],
                              input_params['p'],
                              input_params['metric_params'],  # Remove extra space here
                              input_params['algorithm'],
                              input_params['leaf_size'],
                              test_size=input_params['test'],
                              random_state=42)

        # Return the prediction as JSON
        return jsonify({'KNN': accuracy_knn}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500



@app.route('/svm', methods=['POST'])
def svm_route():
    try:
        # Get the input parameters from the request
        input_params = request.get_json()
        if not input_params:
            return jsonify({'error': 'Invalid input parameters'}), 400
        
        

        # Call the svm() function to make a prediction
        accuracy_svm = SVM(data, input_params['C'],  input_params['kernel'],input_params['gamma'],input_params['coef0'], input_params['tol'], input_params['cache_size'], input_params['max_iter'], input_params['decision_function_shape'],test_size=input_params['test'],random_state=42 )
        # Return the prediction as JSON
        return jsonify({'SVM': accuracy_svm}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500



@app.route('/random', methods=['POST'])
def random_route():
    try:
        # Get the input parameters from the request
        input_params = request.get_json()
        if not input_params:
            return jsonify({'error': 'Invalid input parameters'}), 400
        print(input_params['testR'])  
        # Call the random_forest_retries() function to make a prediction
        accuracy_rf =  RandomForestML(data,input_params['n_estimators'], input_params['max_depth'], input_params['criterion'],  input_params['min_samples_split'], input_params['min_samples_leaf'], input_params['min_weight_fraction_leaf'], input_params['max_leaf_nodes'], input_params['min_impurity_decrease'], input_params['n_jobs'], test_size=input_params['test'], random_state=42 )
        # Return the prediction as JSON
        return jsonify({'RandomForest': accuracy_rf}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500



@app.route('/compare', methods=['POST'])
def compare():
    try:
        # Get the input parameters from the request
        input_params = request.get_json()
        if not input_params:
            return jsonify({'error': 'Invalid input parameters'}), 400
        print(input_params['testR'])  
        # Call the random_forest_retries() function to make a prediction
        accuracy_rf =  RandomForestML(data,input_params['n_estimators'], input_params['max_depth'], input_params['criterion'],  input_params['min_samples_split'], input_params['min_samples_leaf'], input_params['min_weight_fraction_leaf'], input_params['max_leaf_nodes'], input_params['min_impurity_decrease'], input_params['n_jobs'], test_size=input_params['test'], random_state=42 )
        # Return the prediction as JSON
        return jsonify({'RandomForest': accuracy_rf}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500



if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
