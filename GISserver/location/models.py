from django.contrib.gis.db import models
from django.contrib.gis.geos import Point
from django.db import connection

class Urban(models.Model):
    gid = models.IntegerField(primary_key=True)
    scalerank = models.DecimalField(max_digits=10, decimal_places=0)
    featurecla = models.TextField()
    area_sqkm = models.FloatField()
    geom =  models.MultiPolygonField()

class Solar(models.Model):
    value = models.FloatField() # watts per m^2.

    @classmethod
    def get_point(cls, long, lat):
        cursor = connection.cursor()
        cursor.execute("SELECT ST_Value(rast, ST_Point(%s,%s)) AS value FROM solar", (long, lat))
        row = cursor.fetchone()
        solar = cls
        solar.value = row[0]
        return solar

class WindSpeed(models.Model):
    gid = models.IntegerField()
    geom = models.GeometryField()
    total = models.FloatField()

    @classmethod
    def get_point(cls, long, lat):
        p = Point(long, lat)
        return cls.objects.filter(geom__contains=Point).get()

    class Meta:
        db_table = 'windspeed'