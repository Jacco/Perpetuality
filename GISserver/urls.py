# coding=utf-8
from django.conf.urls import patterns, include, url

# Uncomment the next two lines to enable the admin:
# from django.contrib import admin
# admin.autodiscover()

urlpatterns = patterns('',
    # Examples:
    # url(r'^$', 'perpetuallocation.views.home', name='home'),
    # url(r'^perpetuallocation/', include('perpetuallocation.foo.urls')),

    # Uncomment the admin/doc line below to enable admin documentation:
    # url(r'^admin/doc/', include('django.contrib.admindocs.urls')),

    # Uncomment the next line to enable the admin:
    # url(r'^admin/', include(admin.site.urls)),

    url('^$', 'location.views.index'),
    url('^v1/(?P<long>[0-9.\-]+),(?P<lat>[0-9.\-]+)/?$', 'location.views.point_information'),
#    url('ˆ/(?P<long1>\d+),(?P<lat1>\d+)/(?P<long2>\d+),(?P<lat2>\d+)',  'location.views.bounding_box'),
)
