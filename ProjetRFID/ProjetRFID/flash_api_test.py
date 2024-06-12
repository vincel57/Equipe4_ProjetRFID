from flask import Flask, request, jsonify
from ML_RFID_maj import result, knn_classifier, SVM, pre_traitement, unzip_folder, est_fichier_zip, RandomForestML, dataset, comparaison
import os
import zipfile
import datetime
 
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
 
 
@app.route('/fichier', methods=['POST'])
def handle_file_path():
    input_params = request.get_json()
    file_path = input_params['file_path']
    directory = input_params['directory']
    data = None  # Initialiser data a None

    if est_fichier_zip(file_path):
        # Assure-toi que le repertoire de sortie existe, sinon le creer
        if not os.path.exists(directory):
            os.makedirs(directory)

        
            # Decompresser le fichier zip dans le repertoire de sortie
            unzip_folder(file_path, directory)
            # Mettre a jour le chemin du fichier pour pointer vers le repertoire extrait
            file_path = directory

         

file_path='C:/Users/mavin/source/repos/Equipe4_ProjetRFID_Recette/ProjetRFID/ProjetRFID/data_anonymous'

if os.path.exists(file_path):
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
        print(input_params)
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
 
        print(input_params)
        # Call the svm() function to make a prediction
        accuracy_svm = SVM(data, 
                            input_params['C'], 
                            input_params['kernel'],
                            input_params['gamma'],input_params['coef0'], 
                            input_params['tol'], 
                            input_params['cache_size'],
                            input_params['max_iter'], 
                            input_params['decision_function_shape'],
                            test_size=input_params['test'],
                            random_state=42 )

       
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
 
        print(input_params)  
        # Call the random_forest_retries() function to make a prediction
        accuracy_rf =   RandomForestML(data,
                                      input_params['n_estimators'], 
                                      input_params['criterion'],  
                                      input_params['min_samples_split'], 
                                      input_params['min_samples_leaf'], 
                                      input_params['min_weight_fraction_leaf'], 
                                      input_params['max_leaf_nodes'], 
                                      input_params['min_impurity_decrease'], 
                                      input_params['n_jobs'], 
                                      input_params['max_depth'], 
                                      test_size=input_params['test'], 
                                      random_state=42)


                                      
        # Return the prediction as JSON
        return jsonify({'RandomForest': accuracy_rf}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500
 
 
@app.route('/compare', methods=['POST'])
def compare_route():
    try:
        input_params = request.get_json()
        #print(input_params)
        #data = pd.DataFrame(input_params['data'])
        Nom_methode = []
        Valeur_resultats = []   
 
        if input_params['Checkbox_Analytique'] == "Analytique":
            Nom_methode.append("Analytical")
            print(input_params['Checkbox_Analytique'])
            Valeur_resultats.append(input_params['ResultAnalytique'])
 
        if input_params['Checkbox_KNN'] == "KNN":
            Nom_methode.append("KNN")
            Valeur_resultats.append(input_params['ResultKNN'])
 
        if input_params['Checkbox_RandomForest'] == "RandomForest":
            Nom_methode.append("RandomForest")
            Valeur_resultats.append(input_params['ResultRandomForest'])
 
        if input_params['Checkbox_SVM'] == "SVM":
            Nom_methode.append("SVM")
            print(input_params['Checkbox_SVM'])
            Valeur_resultats.append(input_params['ResultSVM'])
            print(Valeur_resultats)
 
        couleur=['orange', None,'red', 'green']
        filepath = comparaison(Valeur_resultats, Nom_methode, couleur)
        print(filepath)
        
       

        return jsonify({'Graphe': filepath}), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500



 

 
if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)