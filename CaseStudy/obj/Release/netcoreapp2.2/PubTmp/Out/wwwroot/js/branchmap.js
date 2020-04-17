﻿// modal component
Vue.component("storemodal", {
    template: "#modal-template",
    props: {
        details: [],
        lat: '',
        lng: ''
    },
    methods: {
        async showModal() {
            try {
                this.modalStatus = "Loading Details..";
                this.showModal = true;
                let response = await fetch(`/GetStores/${this.lat}/${this.lng}`);
                let details = await response.json();
                this.modalStatus = "";
                // only if google gives us the OK
                let myLatLng = new google.maps.LatLng(this.lat, this.lng);
                let map_canvas = document.getElementById("map");
                let options = {
                    zoom: 10, center: myLatLng, mapTypeId:
                        google.maps.MapTypeId.ROADMAP
                };
                let map = new google.maps.Map(map_canvas, options);
                let center = map.getCenter();
                let infowindow = new google.maps.InfoWindow({ content: "" });
                let idx = 0;
                details.map((detail) => {
                    idx = idx + 1;
                    let marker = new google.maps.Marker({
                        position: new google.maps.LatLng(detail.latitude, detail.longitude),
                        map: map,
                        animation: google.maps.Animation.DROP,
                        icon: `../images/marker${idx}.png`,
                        title: `Branch#${detail.id} ${detail.street}, ${detail.city},${detail.region}`,
                        html: `<div>Branch#${detail.id}<br/>${detail.street}, ${detail.city}<br/>${detail.distance.toFixed(2)} km</div>`
                    });
                    google.maps.event.addListener(marker, "click", () => {
                        infowindow.setContent(marker.html); // added .html to the marker object.
                        infowindow.close();
                        infowindow.open(map, marker);
                    });
                });
                map.setCenter(center);
                google.maps.event.trigger(map, "resize");
            } catch (error) {
                console.log(error.statusText);
            }
        }
    },
    mounted() {
        this.showModal();
    }
});
const stores = new Vue({
    el: '#stores',
    methods: {
        loadAndShowModal() {
            try {
                // A service for converting between an address to LatLng
                let geocoder = new google.maps.Geocoder();
                geocoder.geocode({ address: this.address }, (results, status) => {
                    if (status === google.maps.GeocoderStatus.OK) {
                        this.lat = results[0].geometry.location.lat();
                        this.lng = results[0].geometry.location.lng();
                        this.showModal = true;
                    }
                });
            } catch (error) {
                console.log(error);
            }
        }
    },
    data: {
        showModal: false,
        address: '',
        lat: '',
        lng: ''
    }
});