from ibm_watson import NaturalLanguageUnderstandingV1
from ibm_watson.natural_language_understanding_v1 import Features, CategoriesOptions, EntitiesOptions, KeywordsOptions, EmotionOptions, RelationsOptions, SyntaxOptions
from ibm_cloud_sdk_core.authenticators import IAMAuthenticator

import os
import json
from dotenv import load_dotenv
from time import time


def main():
    
    #Example image
    server = 'https://api.eu-gb.natural-language-understanding.watson.cloud.ibm.com'
    IMAGE_API_KEY = os.getenv('NARURALANALYSERAPI')
    naturalLanguageAnalyser = NaturalLanguageUnderstandingV1(version='2018-03-19',authenticator=IAMAuthenticator(IMAGE_API_KEY))
    naturalLanguageAnalyser.set_service_url(server)
    
    
       #Example text
    text = 'Team, I know that times are tough! Product'\
           'sales have been disappointing for the past three '\
           'quarters. We have a competitive product, but we '\
           'need to do a better job of selling it!'

    response = naturalLanguageAnalyser.analyze(text=text,
        features=Features(
            entities=EntitiesOptions(mentions=True, emotion=True, sentiment=True, limit=10),
            emotion=EmotionOptions(),
            keywords=KeywordsOptions(emotion=True, sentiment=True, limit=10),
            relations=RelationsOptions(),
            syntax=SyntaxOptions(sentences=True))).get_result()
    print("Start")
    print(json.dumps(response, indent=2))
    #imageAnalysis = imageAnalyser(url=img_url)
    #for c, img in enumerate(imageAnalysis):
    #    print(f"Image {c+1}:")
    #    for i in img:
    #        print(f"- {i[0]}, {i[1]}")


if __name__ == "__main__":    
    try:
        load_dotenv()
        main()
    except Exception as e:
        print("Error:", e)
