# Create your views here.

import json
from django.http import HttpResponse, HttpResponseBadRequest
from django.shortcuts import render_to_response

from models import Solar

def index(request):
    return render_to_response("index.html")

def point_information(request, long, lat):
    long, lat = sane_position(long, lat)
    if long is None:
        return HttpResponseBadRequest("long, lat out of range")
    solar = Solar.get_point(long, lat)
    data = {'long':long,
            'lat': lat,
            'solar_power': solar.value
    }
    return HttpResponse(json.dumps(data), content_type="application/json")

def sane_position(long, lat):
    long = float(long)
    lat = float(lat)
    if long >= 180 or long < -180 or lat >= 90 or lat < -90:
        return None, None
    return long, lat
