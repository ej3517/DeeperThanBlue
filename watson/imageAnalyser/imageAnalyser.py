from ibm_watson import VisualRecognitionV3
from ibm_cloud_sdk_core.authenticators import IAMAuthenticator

import os
import json
from dotenv import load_dotenv
from time import time

debug = False


def imageAnalyser(url, server='https://api.us-south.visual-recognition.watson.cloud.ibm.com'):
    IMAGE_API_KEY = os.getenv('IMAGEANALYSERAPI')
    image_analyser = VisualRecognitionV3(version='2018-03-19',authenticator=IAMAuthenticator(IMAGE_API_KEY))
    image_analyser.set_service_url(server)
  
    time_start = time()
    classes_result = image_analyser.classify(url=url).get_result()
    time_end = time()
    
    if debug: print(f"Processing Time: {time_end-time_start}")
    if debug: print(json.dumps(classes_result, indent=2))     # Data in - for debug
    
    def sortClasses(lst):
        lst.sort(key = lambda x: x[1], reverse=True)  
        return lst
    img_lst = []
    for img in classes_result["images"]:
        if(len(img["classifiers"]) !=1 ):
            raise Exception("An image has more than 1 classifer. Check why this happend!")
        classes = [(i["class"],i["score"]) for i in img["classifiers"][0]["classes"]]
        img_lst.append(sortClasses(classes))  
    return img_lst


def main():
    
    #Example image
    img_url = 'https://thumbor.forbes.com/thumbor/960x0/https%3A%2F%2Fspecials-images.forbesimg.com%2Fdam%2Fimageserve%2F1072753294%2F960x0.jpg%3Ffit%3Dscale'

    imageAnalysis = imageAnalyser(url=img_url)
    for c, img in enumerate(imageAnalysis):
        print(f"Image {c+1}:")
        for i in img:
            print(f"- {i[0]}, {i[1]}")


if __name__ == "__main__":    
    try:
        load_dotenv()
        main()
    except Exception as e:
        print("Error:", e)