#!/usr/bin/env python
# coding: utf-8

# In[5]:


import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import datetime
import warnings
import zipfile
import os


# In[6]:


warnings.filterwarnings('ignore')


# # Creation de la dataframe reflist

# In[7]:


def est_fichier_zip(chemin_fichier):
    """ Vérifie si le fichier spécifié est un fichier zip en examinant son extension. """
    return os.path.splitext(chemin_fichier)[1].lower() == '.zip'


# In[8]:


def unzip_folder(zip_file, output_dir):
    """
    Décompresse un fichier zip et place les dossiers extraits dans un répertoire spécifié.

    Args:
        zip_file (str): Chemin vers le fichier zip à décompresser.
        output_dir (str): Chemin vers le répertoire de sortie où placer les dossiers extraits.
    """
    # Vérifier si le répertoire de sortie existe, sinon le créer
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    # Ouvrir le fichier zip en mode lecture
    with zipfile.ZipFile(zip_file, 'r') as zip_ref:
        # Extraire tous les fichiers dans le répertoire de sortie
        zip_ref.extractall(output_dir)
   # print("La décompression est terminée.")


# In[81]:


#chemin_fichier = input("Entrez le chemin du fichier à vérifier : ")
#if est_fichier_zip(chemin_fichier):
   # dossier_destination = input("Entrez le chemin du dossier de destination pour la décompression : ")
   # d = dossier_destination
    #   unzip_folder(chemin_fichier, dossier_destination)
  # else:
   # print("Le fichier sélectionné n'est pas un fichier zip.")
    #   d=chemin_fichier
  # pathfile = d


# In[82]:


# reflist: list of epc in each box
reflist = pd.DataFrame()
pathfile=r'C:\Users\mavin\source\repos\Equipe4_ProjetRFIDF\ProjetRFID\ProjetRFID\data_anonymous'
files=os.listdir(pathfile)
for file in files:
    #print(file)
    if file.startswith('reflist_'):
        temp=pd.read_csv(os.path.join(pathfile,file),sep=',').reset_index(drop=True)[['Epc']]
        temp['refListId']=file.split('.')[0]
        reflist = pd.concat([reflist, temp], axis = 0)
        #reflist=reflist.append(temp)
reflist=reflist.rename(columns = {'refListId':'refListId_actual'})
reflist['refListId_actual'] = reflist['refListId_actual'].apply(lambda x:int(x[8:]))

Q_refListId_actual=reflist.groupby('refListId_actual')['Epc'].nunique().rename('Q refListId_actual').reset_index(drop=False)
reflist=pd.merge(reflist,Q_refListId_actual,on='refListId_actual',how='left')
#reflist


# In[84]:


#traiter_fichiers_reflist(d)


# # Creation de la dataframe Tags

# In[85]:


# pathfile=r'data_anonymous'
# 
# df : rfid readings
df=pd.DataFrame()
# 
#files=os.listdir(pathfile)
for file in files:
    if file.startswith('ano_APTags'):
       # print(file)
        temp=pd.read_csv(os.path.join(pathfile,file),sep=',')
        df= pd.concat([df, temp], axis = 0)
df['LogTime'] = pd.to_datetime (df['LogTime'] ,format='%Y-%m-%d-%H:%M:%S') 
df['TimeStamp'] = df['TimeStamp'].astype(float)
df['Rssi'] = df['Rssi'].astype(float)
df=df.drop(['Reader','EmitPower','Frequency'],axis=1).reset_index(drop=True)
df=df[['LogTime', 'Epc', 'Rssi', 'Ant']]
# antennas 1 and 2 are facing the box when photocell in/out 
Ant_loc=pd.DataFrame({'Ant':[1,2,3,4],'loc':['in','in','out','out']})
df=pd.merge(df,Ant_loc,on=['Ant'])
df=df.sort_values('LogTime').reset_index(drop=True)
print(len(df))

#L'ensemble de ces opérations  de la ligne ci dessus est utilisé pour s'assurer que les données dans le DataFrame "df"  
#sont triées en fonctionde leur date et heure de manière croissante et  
#que l'index est réinitialisé pour bien refléter cet ordre de tri.

tags = df
tags


# In[87]:


#traiter_fichiers_rfid(d)


#  COMMENTAIRE: 
# 
# Ce code permet de lire les fichiers commençant par 'ano_APTags' dans le répertoire 'data_anonymous' et de fusionner les données de ces fichiers en un unique DataFrame.
# 
# Le DataFrame résultant contient les colonnes suivantes :
# 
# 'LogTime': la date et l'heure de chaque scan RFID
# 'Epc': l'identifiant unique de chaque produit
# 'Rssi': la puissance du signal RFID capté pour chaque scan
# 'Ant': le numéro d'antenne où a été effectué le scan 'loc': l'emplacement de chaque antenne (entrée ou sortie)
# 
# Le code définit également la table Ant_loc qui permet de faire correspondre les numéros d'antenne aux emplacements d'entrée et de sortie. Enfin, le DataFrame est trié par ordre chronologique des scans et les colonnes inutiles sont supprimées.

# # Creation de la dataframe Windows

# In[88]:


# timing: photocells a time window for each box: start/stop (ciuchStart, ciuchStop)
file=r'ano_supply-process.2019-11-07-CUT.csv'
timing=pd.read_csv(os.path.join(pathfile,file),sep=',')
timing['file']=file
timing['date']=pd.to_datetime(timing['date'],format='%d/%m/%Y %H:%M:%S,%f')
timing['ciuchStart']=pd.to_datetime(timing['ciuchStart'],format='%d/%m/%Y %H:%M:%S,%f')
timing['ciuchStop']=pd.to_datetime(timing['ciuchStop'],format='%d/%m/%Y %H:%M:%S,%f')
timing['timestampStart']=timing['timestampStart'].astype(float)
timing['timestampStop']=timing['timestampStop'].astype(float)
timing=timing.sort_values('date')
timing.loc[:,'refListId']=timing.loc[:,'refListId'].apply(lambda x:int(x[8:]))

timing=timing[['refListId', 'ciuchStart', 'ciuchStop']]

#timing.rename(columns={'refListId': 'windows_id'}, inplace=True)

timing


# In[90]:


#traiter_fichier_timing(d)


# In[91]:


#affiche la taille du modele
len(timing)


# In[92]:


#affiche les 12 premièeres lignes
timing[:12]


# In[93]:


# box 0 always starts
timing[timing['refListId']==0].head()


# In[94]:


runs_box=timing.groupby('refListId').size().rename('runs_box').reset_index(drop=False)
reflist=pd.merge(reflist, runs_box, left_on='refListId_actual', right_on='refListId', how='left').drop(columns=['refListId'])

reflist


# # Mise en commun entre la table tags et reflist

# In[95]:


#on rassemble les deux df à partir de l'epc
df_reflist=pd.merge(tags, reflist, on='Epc', how='left')
df_reflist


# In[96]:


# ciuchStart_up starts upstream ciuchStart, half way in between the previous stop and the actual start
#traitement et modification des données dans le DataFrame timing en calculant de nouveaux attributs et en ajustant les valeurs existantes en fonction de certaines logiques définies.

timing[['ciuchStop_last']]=timing[['ciuchStop']].shift(1)
timing[['refListId_last']]=timing[['refListId']].shift(1)
timing['ciuchStartup']=timing['ciuchStart'] - (timing['ciuchStart'] - timing['ciuchStop_last'])/2 #Formule pour trouver la date du StartUp

# timing start: 10sec before timing
timing.loc[0,'refListId_last']=timing.loc[0,'refListId'] # On met des valeurs au début pour éviter d'avoir des valeurs NULL. Essentiellement, il initialise la colonne 'refListId_last' avec les mêmes valeurs que la colonne 'refListId' pour la première ligne. 
timing.loc[0,'ciuchStartup']=timing.loc[0,'ciuchStart']-datetime.timedelta(seconds=10)
timing.loc[0,'ciuchStop_last']=timing.loc[0,'ciuchStartup']-datetime.timedelta(seconds=10)
timing['refListId_last']=timing['refListId_last'].astype(int)

#  timing stop: 10sec après le timing
timing['ciuchStopdown']= timing['ciuchStartup'].shift(-1)
timing.loc[len(timing)-1,'ciuchStopdown']=timing.loc[len(timing)-1,'ciuchStop']+datetime.timedelta(seconds=10)
timing=timing[['refListId', 'refListId_last','ciuchStartup', 'ciuchStart','ciuchStop','ciuchStopdown']]
timing.head()


# In[97]:


# t0_run = a new run starts when box 0 shows up
# a chaque fois que la box 0 passe, un nouveau run est lancé
t0_run=timing[timing['refListId']==0] [['ciuchStartup']]
t0_run=t0_run.rename(columns={'ciuchStartup':'t0_run'})
t0_run=t0_run.groupby('t0_run').size().cumsum().rename('run').reset_index(drop=False)
t0_run=t0_run.sort_values('t0_run')
# 
# each row in timing is merged with a last row in t0_run where t0_run (ciuchstart) <= timing (ciuchstart)
timing=pd.merge_asof(timing,t0_run,left_on='ciuchStartup',right_on='t0_run', direction='backward')
timing=timing.sort_values('ciuchStop')
timing=timing[['run', 'refListId', 'refListId_last', 'ciuchStartup','ciuchStart','ciuchStop','ciuchStopdown','t0_run']]


# In[98]:


t0_run=t0_run.groupby('t0_run').size()
t0_run


# In[99]:


#  full window (ciuchStartup > ciuchStopdown) is sliced in smaller slices 
# ciuchStartup > ciuchStart: 11 slices named up_0, up_1, ..., up_10 
# ciuchStart > ciuchStop: 11 slices named mid_0, mid_1, ... mid_10
# ciuchStop > ciuchStopdown: 11 slices names down_0, down_1, ... down_10 
# Traduction : Les espaces entre les étapes sont divisées en 11 parties chacune : entre le startUp et le Start ce seront des up_X, entre le Start et le Stop ce seront des mid_X et entre le Stop et le StopDown ce seront des down_X. 
slices=pd.DataFrame() # Création d'un dataframe 

for i, row in timing .iterrows(): # Pour chaque ligne dans timing 
    ciuchStartup=row['ciuchStartup'] # On prend la valeur de ciuchStartup à la ligne i 
    ciuchStart=row['ciuchStart'] # … de ciuchStart à la ligne i 
    ciuchStop=row['ciuchStop'] # idem 
    ciuchStopdown=row['ciuchStopdown'] # idem 
    steps=4 # Nombre de fois qu'on divise entre 2 étapes 
#steps est utilisée pour spécifier le nombre de tranches dans lesquelles la fenêtre de temps doit être divisée. 

#      
    up=pd.DataFrame(index=pd.date_range(start=ciuchStartup, end=ciuchStart,periods=steps,inclusive='left')).reset_index(drop=False).rename(columns={'index':'slice'}) # Divise en 3 parts les instants entre ciuchStart up et ciuchStart (cf image en bas de bloc) 
    up.index=['up_'+str(x) for x in range(steps-1)] # Associe à chaque part un up_X 
    slices=pd.concat([slices,up]) # Ajout des Up dans le dataframe slices 
#De meme  ENTRE START ET STOP 
    mid=pd.DataFrame(index=pd.date_range(start=ciuchStart, end=ciuchStop,periods=steps,inclusive='left')).reset_index(drop=False).rename(columns={'index':'slice'}) 
    mid.index=['mid_'+str(x) for x in range(steps-1)] 
    slices=pd.concat([slices,mid]) 
#     IDEM ENTRE STOP ET STOPDOWN 
    down=pd.DataFrame(index=pd.date_range(start=ciuchStop, end=ciuchStopdown,periods=steps,inclusive='left')).reset_index(drop=False).rename(columns={'index':'slice'}) 
    down.index=['down_'+str(x) for x in range(steps-1)] 
    slices=pd.concat([slices,down]) 
#  slices=slices.append(up)  
slices=slices.reset_index(drop=False).rename(columns={'index':'slice_id'}) # Renomme le nom de la colonne 
#  
timing_slices=pd.merge_asof(slices,timing,left_on='slice',right_on='ciuchStartup',direction='backward') # Jointure des dataframe slices et timing 
timing_slices=timing_slices[['run', 'refListId', 'refListId_last','slice_id','slice',  \
                             'ciuchStartup', 'ciuchStart', 'ciuchStop', 'ciuchStopdown','t0_run']] # Affiche ces colonnes 

timing_slices 


# In[109]:


# merge between df and timing
# merge_asof needs sorted df > df_ref
df=df[ (df['LogTime']>=timing['ciuchStartup'].min()) & (df['LogTime']<=timing['ciuchStopdown'].max())  ]# Sélectionne les données dont la date de scan des articles est supérieure à la plus petite valeur de la colonne ciuchStartUp ET la date de scan des scans est inférieure à la plus grande valeur de ciuchStopDown 
# Ca veut donc dire que c'est impossible que des articles soient scannés alors que la boîte n'est pas encore arrivé au StartUp ou qu'après la dernière boite ait passé par le StopDown. 


df=df.sort_values('LogTime')# Range par ordre chronologique les données selon LogTime
# 
# each row in df_ref is merged with the last row in timing where timing (ciuchstart_up) < df_ref (logtime)
# 
# df_timing=pd.merge_asof(df_ref,timing,left_on=['LogTime'],right_on=['ciuchStartup'],direction='backward')
# df_timing=df_timing.dropna()
# df_timing=df_timing.sort_values('LogTime').reset_index(drop=True)
# df_timing=df_timing[['run', 'Epc','refListId', 'refListId_last', 'ciuchStartup',\
#                      'LogTime', 'ciuchStop', 'ciuchStopdown','Rssi', 'loc', 'refListId_actual']]
# 
# each row in df_ref is merged with the last row in timing_slices where timing (slice) < df_ref (logtime)
# 
df_timing_slices=pd.merge_asof(df,timing_slices,left_on=['LogTime'],right_on=['slice'],direction='backward')
df_timing_slices=df_timing_slices.dropna()
df_timing_slices=df_timing_slices.sort_values('slice').reset_index(drop=True)
df_timing_slices=df_timing_slices[['run', 'Epc','refListId', 'refListId_last', 'ciuchStartup','slice_id','slice','LogTime', \
                      'ciuchStart','ciuchStop', 'ciuchStopdown', 'Rssi', 'loc','t0_run']]
df_timing_slices['reflist_run_id'] = df_timing_slices['refListId'].astype(str) +"_"+ df_timing_slices['run'].astype(str)
df_timing_slices


# tags : Dataframe contenant les articles scannés 
# timing_slices : Dataframe contenant les boîtes scannées avec les parties divisées 
# tags_times_slices : Dataframe contenant les articles scannés ainsi que les boîtes avec les parties divisées


# In[110]:


runs_out=df_timing_slices.groupby('run')['refListId'].nunique().rename('Q refListId').reset_index(drop=False)

#groupe les données dans tags_timing_slices par la colonne "run", 
#compte le nombre de valeurs uniques de "refListId" pour chaque groupe, renomme la colonne résultante "Q refListId", et réinitialise l'index.

#runs_out[runs_out['Q refListId']!=10]#filtre le dataframe pour n'afficher que les lignes où "Q refListId" n'est pas égal à 10.

#En résumé cette opération cherche à vérifier si pour chaque groupe "run", il y a exactement 10 valeurs uniques de "refListId".
#Si ce n'est pas le cas, alors les lignes correspondantes seront affichées

runs_out


# In[111]:


#suppression  des doublons dans la combinaison de colonnes ('run', 'refListId', 'refListId_last') de timing_slices, 
# et on ne garde que les colonnes 'run', 'refListId', 'refListId_last' et 'ciuchStop'

current_last_windows=timing_slices.drop_duplicates(['run','refListId','refListId_last'])
current_last_windows=current_last_windows[['run','refListId','refListId_last','ciuchStop']].reset_index(drop=True)
current_last_windows


# In[112]:


# runs 16 23 32 40 have missing boxes: discarded
# also run 1 is the start, no previous box: discarded
# run 18: box 0 run at the end
# 
timing=timing[~timing['run'].isin([1,18,16,23,32,40])]
timing_slices=timing_slices[~timing_slices['run'].isin([1,18,16,23,32,40])]
df_timing_slices=df_timing_slices[~df_timing_slices['run'].isin([1,18,16,23,32,40])]

df_timing_slices=df_timing_slices.sort_values(['LogTime','Epc'])
# 


# In[113]:


len(timing),len(timing_slices), len(df_timing_slices)


# In[114]:


# df_timing_slices['dt']=
df_timing_slices['dt']=(df_timing_slices['LogTime']-df_timing_slices['t0_run']).apply(lambda x:x.total_seconds())

df_timing_slices


# In[115]:


rssi_threshold=-110
df_timing_slices_threshold=df_timing_slices[df_timing_slices['Rssi']>rssi_threshold]# df_timing_slices_threshold est un dataframe 
#contenant toutes les données ayant une puissance de signal supérieure à -110

df_timing_slices_threshold


# In[116]:


# readrate
# readrate
round(100*df_timing_slices_threshold.reset_index(drop=False).groupby(['run','loc'])
      ['Epc'].nunique().groupby('loc').mean()/reflist['Epc'].nunique(),2)


# # Méthode Analytique

# In[117]:


def analytical(df_timing_slices, timing_slices):
    
# La fonction calcule la valeur maximale de Rssi pour chaque combinaison d''Epc', 'refListId', 'subslice_id' et 'loc' à partir du DataFrame tags. Les valeurs sont stockées dans un DataFrame nommé ana.
    ana = df_timing_slices.groupby(['Epc','reflist_run_id','slice_id','loc'])['Rssi'].max()\
            .unstack('loc', fill_value=-110).reset_index(drop = False)
    
# La fonction crée un DataFrame nommé order contenant les valeurs uniques de 'subslice_id' et leur ordre d'apparition 'order'.  
    order = pd.DataFrame(timing_slices['slice_id'].unique(), columns = ['slice_id'])
    order['order'] = order.index
    
# La fonction fusionne le DataFrame ana avec le DataFrame order sur la colonne 'subslice_id' pour obtenir une colonne order dans ana.
    ana = pd.merge(ana, order, on = 'slice_id', how = 'left')
    ana = ana [['Epc','reflist_run_id','slice_id','in','out','order']]
    
# La fonction extrait le dernier 'subslice_id' avec une valeur de 'out' supérieure à 'in', et le premier 'subslice_id' avec une valeur de 'in' supérieure à 'out' à partir de ana. 
# Ces deux ensembles de données sont stockés dans les DataFrames 'ana_out' et 'ana_in'.
    
# Last subslice_id with out>in (pas encore à l'intérieur)
    ana_out = ana[ana['out']>ana['in']].sort_values(['Epc','reflist_run_id','order'], ascending = False).drop_duplicates(['Epc','reflist_run_id'])

# First subslice_id with in>out (à dépasser l'intérieur)
    ana_in = ana[ana['in']>ana['out']].sort_values(['Epc','reflist_run_id','order'], ascending = True).drop_duplicates(['Epc','reflist_run_id'])

# La fonction fusionne les DataFrames ana_in et ana_out sur les colonnes Epc et refListId en utilisant une jointure interne (inner join). Le résultat est stocké dans le DataFrame ana.
# Rajoute un _in en suffix pour les élements fusionnés venant de anna_in et réciproquement avec _out
    ana = pd.merge(ana_in, ana_out, on=['Epc', 'reflist_run_id'], suffixes=['_IN', '_OUT'], how = 'inner') \
            .sort_values(['Epc'])
# La fonction fusionne le DataFrame ana avec un autre DataFrame nommé reflist sur la colonne Epc en utilisant une jointure gauche (left join). Le résultat est stocké dans le DataFrame ana.
    ana = pd.merge(ana, reflist, on = 'Epc', how = 'left')

# La fonction ajoute une colonne 'pred_ana_bool' à ana qui contient la valeur booléenne True si la partie initiale de la chaîne 'refListId' (avant le premier caractère "_") est égale à la valeur de la colonne box de ana.
    ana['pred_ana_bool'] = ana['reflist_run_id'].apply(lambda x:x.split('_')[0]).astype('int64') == ana['refListId_actual']

#  Nombre de ligne pour la colonne 'pred_ana_bool' dans le df ana
    value_counts = ana['pred_ana_bool'].value_counts()
    
# On calcul le poucentage de true dans 'pred_ana_bool' donc le nombre de prédiction vrai
    true_percentage = value_counts[True] / value_counts.sum()*100

# La fonction retourne le poucentage de prédiction vraie
    return true_percentage


#analytical(df_timing_slices, timing_slices)


# In[118]:


score4=analytical(df_timing_slices, timing_slices)
score4


# In[34]:


def result():
    return analytical(df_timing_slices, timing_slices)


# # Machine learning

# In[119]:


# 
# ds : 
# sample: one tag in one window 
# 

def dataset(df_timing_slices, windows, rssi_quantite, reflist):
    ds_rssi = df_timing_slices.groupby(['Epc', 'reflist_run_id', 'slice_id', 'loc'])['Rssi'].quantile(rssi_quantite) \
                 .unstack(['slice_id', 'loc'], fill_value=-110)
    ds_rssi.columns = [x[0] + '_' + x[1] for x in ds_rssi.columns]
    ds_rssi = ds_rssi.reset_index(drop=False)
    
    ds_rc = df_timing_slices.groupby(['Epc', 'reflist_run_id', 'slice_id', 'loc']).size() \
               .unstack(['slice_id', 'loc'], fill_value=0)
    ds_rc.columns = [x[0] + '_' + x[1] for x in ds_rc.columns]
    ds_rc = ds_rc.reset_index(drop=False)
    
    ds = pd.merge(ds_rssi, ds_rc, on=['Epc', 'reflist_run_id'], suffixes=['_rssi', '_rc'])
    ds = pd.merge(ds, windows, on='reflist_run_id', how='left')
    
    Q_Epcs_window = df_timing_slices.groupby(['reflist_run_id'])['Epc'].nunique().rename('Epcs_window').reset_index(drop=False)
    ds = pd.merge(ds, Q_Epcs_window, on='reflist_run_id', how='left')
    
    Q_reads_window = df_timing_slices.groupby(['reflist_run_id']).size().rename('reads_window').reset_index(drop=False)
    ds = pd.merge(ds, Q_reads_window, on='reflist_run_id', how='left')
    
    ds=pd.merge(ds, reflist, on='Epc', how='left')
    ds['actual']=ds['reflist_run_id'].apply(lambda x: x.split ('_')[0]).astype('int64')== ds['refListId_actual']
    ds['actual'] = ds ['actual'].replace({True: 'IN' , False: 'OUT'})

    
    return ds


# In[122]:


def Xcols_func(features, Xcols_all):
    Features=pd.DataFrame(\
        [\
         ['all', True, True, False, True, True, True],\
         ['rssi & rc only', True, True, False, False, False, False],\
         ['rssi & rc_mid', True, True, True, False, False, False],\
         ['rssi only', True, False, True, False, False, False],\
         ['rc only', False, True, False, False, False, False],\
        ], columns=['features', 'rssi', 'rc', 'rc_mid_only', 'Epcs_window', 'reads_window', 'window_width'])
    Features

    Features_temp = Features[Features['features']==features]
    
    X=[]
    rssi = Features_temp ['rssi'].values[0]
    rc = Features_temp['rc'].values[0]
    rc_mid_only = Features_temp['rc_mid_only'].values[0]
    Epcs_window =  Features_temp['Epcs_window'].values[0]
    reads_window =  Features_temp['reads_window'].values[0]
    window_width =  Features_temp['window_width'].values[0]
    
    colonne = Xcols_all

    X_rssi = [x for x in colonne if rssi*'rssi' in x.split('_')]
    X_rc = [x for x in colonne if rc*'rc' in x.split('_')]
    
    X = X_rssi + X_rc
    
    if Epcs_window:
        X.append('Epcs_window')
    if reads_window:
        X.append('reads_window')
    if window_width:
        X.append('window_width')
        
    return X


# In[120]:


def pre_traitement(path):
    
    pathfile = path
    print(pathfile)
    # reflist: list of epc in each box
    reflist = pd.DataFrame()
    # 
    files=os.listdir(pathfile)
    for file in files:
        if file.startswith('reflist_'):
            temp=pd.read_csv(os.path.join(pathfile,file),sep=',').reset_index(drop=True)[['Epc']]
            temp['refListId']=file.split('.')[0]
            reflist = pd.concat([reflist, temp], axis = 0)
            #reflist=reflist.append(temp)
    reflist=reflist.rename(columns = {'refListId':'refListId_actual'})
    reflist['refListId_actual'] = reflist['refListId_actual'].apply(lambda x:int(x[8:]))

    Q_refListId_actual=reflist.groupby('refListId_actual')['Epc'].nunique().rename('Q refListId_actual').reset_index(drop=False)
    reflist=pd.merge(reflist,Q_refListId_actual,on='refListId_actual',how='left')
    
    df=pd.DataFrame()
    # 
    #files=os.listdir(pathfile)
    for file in files:
        if file.startswith('ano_APTags'):
       
            temp=pd.read_csv(os.path.join(pathfile,file),sep=',')
            df= pd.concat([df, temp], axis = 0)
    df['LogTime'] = pd.to_datetime (df['LogTime'] ,format='%Y-%m-%d-%H:%M:%S') 
    df['TimeStamp'] = df['TimeStamp'].astype(float)
    df['Rssi'] = df['Rssi'].astype(float)
    df=df.drop(['Reader','EmitPower','Frequency'],axis=1).reset_index(drop=True)
    df=df[['LogTime', 'Epc', 'Rssi', 'Ant']]
    # antennas 1 and 2 are facing the box when photocell in/out 
    Ant_loc=pd.DataFrame({'Ant':[1,2,3,4],'loc':['in','in','out','out']})
    df=pd.merge(df,Ant_loc,on=['Ant'])
    df=df.sort_values('LogTime').reset_index(drop=True)

    tags = df

        # timing: photocells a time window for each box: start/stop (ciuchStart, ciuchStop)
    file=r'ano_supply-process.2019-11-07-CUT.csv'
    timing=pd.read_csv(os.path.join(pathfile,file),sep=',')
    timing['file']=file
    timing['date']=pd.to_datetime(timing['date'],format='%d/%m/%Y %H:%M:%S,%f')
    timing['ciuchStart']=pd.to_datetime(timing['ciuchStart'],format='%d/%m/%Y %H:%M:%S,%f')
    timing['ciuchStop']=pd.to_datetime(timing['ciuchStop'],format='%d/%m/%Y %H:%M:%S,%f')
    timing['timestampStart']=timing['timestampStart'].astype(float)
    timing['timestampStop']=timing['timestampStop'].astype(float)
    timing=timing.sort_values('date')
    timing.loc[:,'refListId']=timing.loc[:,'refListId'].apply(lambda x:int(x[8:]))
    timing=timing[['refListId', 'ciuchStart', 'ciuchStop']]
    
    
        # ciuchStart_up starts upstream ciuchStart, half way in between the previous stop and the actual start
    timing[['ciuchStop_last']]=timing[['ciuchStop']].shift(1)
    timing[['refListId_last']]=timing[['refListId']].shift(1)
    timing['ciuchStartup']=timing['ciuchStart'] - (timing['ciuchStart'] - timing['ciuchStop_last'])/2
    # timing start: 10sec before timing
    timing.loc[0,'refListId_last']=timing.loc[0,'refListId']
    timing.loc[0,'ciuchStartup']=timing.loc[0,'ciuchStart']-datetime.timedelta(seconds=10)
    timing.loc[0,'ciuchStop_last']=timing.loc[0,'ciuchStartup']-datetime.timedelta(seconds=10)
    timing['refListId_last']=timing['refListId_last'].astype(int)
    # 
    timing['ciuchStopdown']= timing['ciuchStartup'].shift(-1)
    timing.loc[len(timing)-1,'ciuchStopdown']=timing.loc[len(timing)-1,'ciuchStop']+datetime.timedelta(seconds=10)
    timing=timing[['refListId', 'refListId_last','ciuchStartup', 'ciuchStart','ciuchStop','ciuchStopdown']]
    
    
        # t0_run = a new run starts when box 0 shows up
    t0_run=timing[timing['refListId']==0] [['ciuchStartup']]
    t0_run=t0_run.rename(columns={'ciuchStartup':'t0_run'})
    t0_run=t0_run.groupby('t0_run').size().cumsum().rename('run').reset_index(drop=False)
    t0_run=t0_run.sort_values('t0_run')
    # 
    # each row in timing is merged with a last row in t0_run where t0_run (ciuchstart) <= timing (ciuchstart)
    timing=pd.merge_asof(timing,t0_run,left_on='ciuchStartup',right_on='t0_run', direction='backward')
    timing=timing.sort_values('ciuchStop')
    timing=timing[['run', 'refListId', 'refListId_last', 'ciuchStartup','ciuchStart','ciuchStop','ciuchStopdown','t0_run']]

    
    slices = pd.DataFrame()
    for i, row in timing.iterrows():
        ciuchStartup = row['ciuchStartup']
        ciuchStart = row['ciuchStart']
        ciuchStop = row['ciuchStop']
        ciuchStopdown = row['ciuchStopdown']
        steps = 3

        # Création des tranches "up"
        up = pd.DataFrame(index=pd.date_range(start=ciuchStartup, end=ciuchStart, periods=steps)) \
            .reset_index(drop=False).rename(columns={'index': 'slice'})
        up['slice_id'] = ['up_' + str(x) for x in range(steps)]
        slices = pd.concat([slices, up], ignore_index=True)

        # Création des tranches "mid"
        mid = pd.DataFrame(index=pd.date_range(start=ciuchStart, end=ciuchStop, periods=steps)) \
            .reset_index(drop=False).rename(columns={'index': 'slice'})
        mid['slice_id'] = ['mid_' + str(x) for x in range(steps)]
        slices = pd.concat([slices, mid], ignore_index=True)

        # Création des tranches "down"
        down = pd.DataFrame(index=pd.date_range(start=ciuchStop, end=ciuchStopdown, periods=steps)) \
            .reset_index(drop=False).rename(columns={'index': 'slice'})
        down['slice_id'] = ['down_' + str(x) for x in range(steps)]
        slices = pd.concat([slices, down], ignore_index=True)

    slices.reset_index(drop=False, inplace=True)

    # 
    timing_slices=pd.merge_asof(slices,timing,left_on='slice',right_on='ciuchStartup',direction='backward')
    timing_slices=timing_slices[['run', 'refListId', 'refListId_last','slice_id','slice',  \
                                 'ciuchStartup', 'ciuchStart', 'ciuchStop', 'ciuchStopdown','t0_run']]
    
    # merge between df and timing
    # merge_asof needs sorted df > df_ref
    df=df[ (df['LogTime']>=timing['ciuchStartup'].min()) & (df['LogTime']<=timing['ciuchStopdown'].max())  ]
    df=df.sort_values('LogTime')
    # 
    # each row in df_ref is merged with the last row in timing where timing (ciuchstart_up) < df_ref (logtime)
    # 
    # df_timing=pd.merge_asof(df_ref,timing,left_on=['LogTime'],right_on=['ciuchStartup'],direction='backward')
    # df_timing=df_timing.dropna()
    # df_timing=df_timing.sort_values('LogTime').reset_index(drop=True)
    # df_timing=df_timing[['run', 'Epc','refListId', 'refListId_last', 'ciuchStartup',\
    #                      'LogTime', 'ciuchStop', 'ciuchStopdown','Rssi', 'loc', 'refListId_actual']]
    # 
    # each row in df_ref is merged with the last row in timing_slices where timing (slice) < df_ref (logtime)
    # 
    df_timing_slices=pd.merge_asof(df,timing_slices,left_on=['LogTime'],right_on=['slice'],direction='backward')
    df_timing_slices=df_timing_slices.dropna()
    df_timing_slices=df_timing_slices.sort_values('slice').reset_index(drop=True)
    df_timing_slices=df_timing_slices[['run', 'Epc','refListId', 'refListId_last', 'ciuchStartup','slice_id','slice','LogTime', \
                          'ciuchStart','ciuchStop', 'ciuchStopdown', 'Rssi', 'loc','t0_run']]
    df_timing_slices['reflist_run_id'] = df_timing_slices['refListId'].astype(str) +"_"+ df_timing_slices['run'].astype(str)
    
    # runs 16 23 32 40 have missing boxes: discarded
    # also run 1 is the start, no previous box: discarded
    # run 18: box 0 run at the end
    # 
    timing=timing[~timing['run'].isin([1,18,16,23,32,40])]
    timing_slices=timing_slices[~timing_slices['run'].isin([1,18,16,23,32,40])]
    df_timing_slices=df_timing_slices[~df_timing_slices['run'].isin([1,18,16,23,32,40])]

    df_timing_slices=df_timing_slices.sort_values(['LogTime','Epc'])
    
    # df_timing_slices['dt']=
    df_timing_slices['dt']=(df_timing_slices['LogTime']-df_timing_slices['t0_run']).apply(lambda x:x.total_seconds())
    
    timing['reflist_run_id']= timing['refListId'].astype(str)+"_"+ timing['run'].astype(str)
    timing['window_width']=(timing['ciuchStopdown']-timing['ciuchStartup']).apply(lambda x:x.total_seconds())
    windows=timing[['reflist_run_id', 'window_width']]
    rssi_quantite=1
    return [df_timing_slices, windows,rssi_quantite,reflist]


# In[121]:


#data=pre_traitement(d)


# # Importation des bibliothèques

# In[124]:


from sklearn.datasets import make_classification
from sklearn.tree import DecisionTreeClassifier
from sklearn.preprocessing import MinMaxScaler
from sklearn.metrics import accuracy_score
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.model_selection import cross_val_score
from sklearn.model_selection import cross_val_predict
from sklearn.neighbors import KNeighborsClassifier
from sklearn.preprocessing import LabelEncoder


# # Fonction RandomForest

# In[133]:


# n_estimators est le nombre d'arbres de décision dans la forêt aléatoire. Controle la complexité et sa performance. 
# Plus on augmente plus temps de calcul long et risque de surapprentissage.

# max_depth c'est la profondeur maximale de chaque arbre de décision dans la forêt. 
# Il contrôle la complexité de chaque arbre individuel dans la forêt.
# max_depth plus grand capture plus de relations complexes entre les variables d'entrée, 
# mais augmente le risque de surapprentissage et réduit la capacité de généralisation.

def RandomForestML(ds, nb_arbre, max_profondeur, test_size,
                   criterion='gini', min_samples_split=2, min_samples_leaf=1, 
                   min_weight_fraction_leaf=0.0, max_leaf_nodes=None, 
                   min_impurity_decrease=0.0, n_jobs=None, random_state=None):

    # Préparation et mise à l'échelle des données
    X = ds[Xcols_func('rssi & rc only', ds.columns)]
    y = LabelEncoder().fit_transform(ds['actual'])

    # Appliquer le MinMaxScaler aux données
    scaler = MinMaxScaler()
    X_scaled = scaler.fit_transform(X)
    # Classificateur RandomForest
    rf = RandomForestClassifier(
        n_estimators=nb_arbre, max_depth=max_profondeur, criterion=criterion,
        min_samples_split=min_samples_split, min_samples_leaf=min_samples_leaf,
        min_weight_fraction_leaf=min_weight_fraction_leaf, max_leaf_nodes=max_leaf_nodes,
        min_impurity_decrease=min_impurity_decrease, n_jobs=n_jobs, random_state=random_state
    )
    y_pred = cross_val_predict(rf, X_scaled, y, cv=20)

    # Évaluation des performances avec validation croisée
    accuracies = cross_val_score(rf, X_scaled, y, cv=5, scoring='accuracy')
    accuracy = accuracies.mean()
    
    return accuracy*100


# In[134]:


#  # Fonction KNN

# In[127]:


def knn_classifier(ds:pd.DataFrame,n_neighbors, weight, metric, p, metric_params, algorithm, leaf_size, test_size, random_state=None):
    # Préparation et mise à l'échelle des données
    X = ds[Xcols_func('rssi & rc only', ds.columns)]
    y = LabelEncoder().fit_transform(ds['actual'])

    # Appliquer le MinMaxScaler aux données
    scaler = MinMaxScaler()
    X_scaled = scaler.fit_transform(X)

    # Création d'une instance du classificateur KNN avec les hyperparamètres spécifiés
    knn = KNeighborsClassifier(
        n_neighbors=n_neighbors, weights=weight, metric=metric, p=p, 
        metric_params=metric_params, algorithm=algorithm, leaf_size=leaf_size
    )
    
    y_pred = cross_val_predict(knn, X_scaled, y, cv=20)

    # Évaluation des performances avec validation croisée
    accuracies = cross_val_score(knn, X_scaled, y, cv=5, scoring='accuracy')
    accuracy = accuracies.mean()
    
    return accuracy*100


# In[128]:


# # Fonction SVC 

# In[129]:


from sklearn.svm import SVC
from sklearn.metrics import accuracy_score
from sklearn.model_selection import train_test_split

def SVM(ds,C, kernel, gamma, coef0, tol, cache_size, max_iter, decision_function_shape, test_size, random_state=42):
    # Préparation et mise à l'échelle des données
    # Préparation et mise à l'échelle des données
    X = ds[Xcols_func('rssi & rc only', ds.columns)]
    y = LabelEncoder().fit_transform(ds['actual'])

    # Appliquer le MinMaxScaler aux données
    scaler = MinMaxScaler()
    X_scaled = scaler.fit_transform(X)
 
    # Créer un modèle SVC avec les hyperparamètres définis
    svc = SVC(
        C=C, kernel=kernel, gamma=gamma, coef0=coef0, tol=tol, 
        cache_size=cache_size, max_iter=max_iter, 
        decision_function_shape=decision_function_shape, random_state=random_state
    )
        
    y_pred = cross_val_predict(svc, X_scaled, y, cv=20)

    # Évaluation des performances avec validation croisée
    accuracies = cross_val_score(svc, X_scaled, y, cv=5, scoring='accuracy')
    accuracy = accuracies.mean()
    
    return accuracy*100



# In[130]:


# # Comparaison

# In[131]:


import matplotlib.pyplot as plt
from matplotlib.ticker import FixedLocator
import numpy as np

def comparaison(resultats, noms_methodes, couleurs):
    # Conversion de la liste des résultats en pourcentage
   # resultats = [resultat * 100 for resultat in resultats]

    # Création des barres
    positions = np.arange(len(noms_methodes))
    barres = plt.bar(positions, resultats, align='center', width=0.5, edgecolor='black')

    # Attribution des couleurs aux barres
    for i, (bar, couleur) in enumerate(zip(barres, couleurs)):
        if couleur is not None:
            bar.set_color(couleur)

    # Configuration des axes et du titre
    plt.ylabel("Accuracy (%)")
    plt.xlabel("Méthodes de prédiction")
    plt.title("Comparaison entre méthode Analytique et Machine Learning")
    plt.xticks(positions, noms_methodes)
    
    # Limitation des valeurs de l'axe y et ajout des étiquettes de pourcentage
    plt.ylim(bottom=0, top=100)
    yticks = np.arange(0, 101, 10)
    plt.gca().yaxis.set_major_locator(FixedLocator(yticks))
    plt.gca().set_yticklabels(['{:.0f}%'.format(label) for label in yticks])
 
    # Affichage des valeurs au-dessus des barres
    for i, resultat in enumerate(resultats):
        plt.text(i, resultat + 1, '{:.2f}%'.format(resultat), ha='center', va='bottom')

    # Affichage du graphique
    plt.show()


# In[135]:


#resultats = [score1,score2,score3,score4]
#comparaison(resultats, [" RF ", "KNN", "SVM", "Analytique"], ['orange', None,'red', 'green'])

