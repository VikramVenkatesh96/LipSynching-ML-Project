from flask import Flask
from flask import request
import json
from flask_cors import CORS, cross_origin
import traceback
from waitress import serve
import chatbot                                      #Check chatbot.py for any modifications

app = Flask(__name__)
# app.config['CORS_HEADERS'] = 'Content-Type'
#
# cors = CORS(app, resources={r"/predict_petal_length": {"origins": "*"}})


@app.route('/predict_viseme', methods=["GET"])
@cross_origin(origin='*', headers=['Content- Type'])
def predict_viseme():
    try:
        req = request.args.get('req')
        grapheme=chatbot.getResponse(str(req))
        viseme= chatbot.getViseme(grapheme)
        response_string= grapheme + "\n" +viseme
        return response_string

    except Exception:
        return traceback.format_exc()


@app.route("/")
def main():
    return "Welcome1"


if __name__ == "__main__":
    # app.run() ##Replaced with below code to run it using waitress
    serve(app, host='0.0.0.0', port=8000) #Change here to use a different server(localhost by default)